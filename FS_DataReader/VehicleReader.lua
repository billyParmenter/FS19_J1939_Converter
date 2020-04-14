--
-- 	FILENAME:		Reader
--	DATE:			April 13th 2020
--	PROGRAMMER:		Olorutoba Samuel Lagunju
--					With aid from Taylor Beck
--	DESCRIPTION:	Extracts vehicle data from FS19 to data.txt file 
--
--
 
--
-- 	Class: 			VehicleReader 
--	Description:	Contains essentials variables that tracks a vehicle's data.
--
VehicleReader = {
    active = nil,
}

VehicleReader.setonce = false
-- can not be 0. This fuel use data are from AG service agencies: 6 gal per operating hour per 100 HP
VehicleReader.basevalue = 6.0 

-- can not be 0. Use this value to adjust the fuel usage to your preference
VehicleReader.usemodifier = 2.0 

-- Enable DEF usage. DONT! Not until Giants enables it.
VehicleReader.usedef = false

function VehicleReader.prerequisitesPresent(specializations)
    return true;
end
--From: http://www.farmingsimulator19mods.com/realistic-fuel-usage-v1-0-0-0-fs19/
function VehicleReader:setDefaults()
	local use = 0;
	if (VehicleReader.vehicle ~= nil) then
		v = VehicleReader.vehicle;
		if (VehicleReader.basevalue < 0) then
			VehicleReader.basevalue = 6.0
		end;
		if (VehicleReader.basevalue > 36) then
			VehicleReader.basevalue = 36
		end;
		if (VehicleReader.usemodifier < 0) then
			VehicleReader.usemodifier = 0.01
		end;
		if (VehicleReader.usemodifier > 10) then
			VehicleReader.usemodifier = 10
		end;
		-- 3.786 litres = 1 gallon
		local ubase = (3.786 * VehicleReader.basevalue) / 3600000
		
		if v ~= nil then	
			if (v.spec_motorized ~= nil) then
				local mpow = (v.spec_motorized.motor.peakMotorTorque * 0.85) + 0.5 -- normalized 
				if v.spec_motorized.consumersByFillType[FillType.DIESEL] ~= nil then -- belt fix
					local realuse = (ubase * mpow) -- 6 gal/hr / 100hp in ms
					use = realuse * VehicleReader.usemodifier;
				end;
				if (VehicleReader.usedef == true) then
					if v.spec_motorized.consumersByFillType[FillType.DEF] ~= nil then -- belt fix
						local defuse = ((ubase / 40) * mpow) -- standard OEM 1:40 ratio
					end;
				end;
			end;
		end;		
	end;
	return use;
end;

function VehicleReader:loadMap()
	g_currentMission.environment:addHourChangeListener(VehicleReader);
	VehicleReader.vehicle = nil;	
	VehicleReader.dumpTimer = 0;
	VehicleReader.fuelTimer = 0;
	VehicleReader.operatingTime = 0; -- current operating time - reset every hour
	VehicleReader.curFuelUsed = 0; -- track hourly consumption
	VehicleReader.totalFuelUsed = 0; -- track total consumption since beginning of operation
	VehicleReader.hour = (1000 * 60 * 60);
	VehicleReader.secondRatio = .00027; -- ratio of second to hour
	
	VehicleReader.requestedTracking = false;
	
end

function VehicleReader:onInputEnter(_, inputValue)
	print("Entered Vehicle");
end

function VehicleReader:deleteMap()
end

function VehicleReader:SetTracking(setActive)
	if setActive then		
		VehicleReader.vehicle = g_currentMission.controlledVehicle;	
		VehicleReader.dumpTimer = 0;
	end;		
end

function VehicleReader:EndTracking(endTracking)
	if endTracking then		
		VehicleReader.vehicle = nil;
		VehicleReader.requestedTracking = true;
		VehicleReader.operatingTime = 0;
		VehicleReader.fuelTimer = 0;
	end;	
end

function VehicleReader:keyEvent(unicode, sym, modifier, isDown)
	if Input.isKeyPressed(Input.KEY_lalt) and Input.isKeyPressed(Input.KEY_q) then
		if VehicleReader.vehicle ~= nil then
			g_gui:showYesNoDialog({text="Quit sending data from " .. VehicleReader.vehicle:getName() .. "?" , title="Osiris Data Reader", callback=self.EndTracking, target=self}); 
		end 
	end;
end

function VehicleReader:update(dt)
	if g_currentMission.controlledVehicle ~= nil then
		if VehicleReader.vehicle == nil and VehicleReader.requestedTracking == false then
			g_gui:showYesNoDialog({text="Do you want to begin sending data from " .. g_currentMission.controlledVehicle:getName() .. "?\nPress alt + q to quit", title="Osiris Data Reader", callback=self.SetTracking, target=self}); 	
			VehicleReader.requestedTracking = true;
		end;	
	else	
		VehicleReader.requestedTracking = false;
	end;

	
	if VehicleReader.vehicle ~= nil and VehicleReader.dumpTimer >= 1000 then	
		local file, err = io.open ("data.txt","w");
		if file ~= nil then
			-- file:write(DebugUtil.printTableRecursively(g_currentMission,".",0,5));
			file:write(tostring(VehicleReader.vehicle:getName()) .. "," .. g_i18n:getSpeed(tostring(VehicleReader.vehicle:getLastSpeed())) .. "," .. tostring(VehicleReader.setDefaults() * 3600000 / 3.786 ) .. "," .. tostring(VehicleReader.setDefaults() * g_currentMission.missionInfo.timeScale));
			file:close();
		end;		
		VehicleReader.dumpTimer = 0;
	end;

	if VehicleReader.vehicle ~= nil and VehicleReader.fuelTimer >= 1000 then
		VehicleReader.usemodifier = 1.0;
		local consumptionOffset;
		if VehicleReader.GetTerrainType() == "Field" then
			consumptionOffset = .25;
		elseif VehicleReader.GetTerrainType() == "Road" then
			consumptionOffset = 0;
		else 
			consumptionOffset = .12;
		end;		 -- modify consumption based on terrain
		VehicleReader.usemodifier = VehicleReader.usemodifier + consumptionOffset;
		VehicleReader.curFuelUsed = VehicleReader.setDefaults() * g_currentMission.missionInfo.timeScale;
		VehicleReader.totalFuelUsed = VehicleReader.totalFuelUsed + VehicleReader.curFuelUsed;
		VehicleReader.fuelTimer = 0;
	end;

	VehicleReader.dumpTimer = VehicleReader.dumpTimer + dt;
	VehicleReader.fuelTimer = VehicleReader.fuelTimer + dt;
end

function VehicleReader:VehicleOnRoad ()
	local onRoad = false;
	if VehicleReader.vehicle ~= nil then
		for _,wheel in ipairs(VehicleReader.vehicle:getWheels()) do
			onRoad = wheel.contact == Wheels.WHEEL_OBJ_CONTACT;
			if onRoad then
				break;
			end;
		end;
	end;
	return onRoad;
end

function VehicleReader:GetTerrainType()				
		local terrainType = "--";
		if VehicleReader.vehicle ~= nil then 
			local onRoad  = VehicleReader.VehicleOnRoad();
			if  VehicleReader.vehicle:getIsOnField() then
				terrainType = "Field";
			elseif onRoad then
				terrainType = "Road";
			else 
				terrainType = "Offroad"
			end;
		end;
		return terrainType;
end
addModEventListener(VehicleReader);