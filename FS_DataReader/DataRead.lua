--
-- Data Read
--
-- Dump vehicle data to file 
--
-- @author  	Samuel Lagunju
-- @version 	v1
-- @date  		01/18/2020
--
--


--
-- DataRead
--
DataRead = {
    active = nil,
}

DataRead.setonce = false
-- can not be 0. This fuel use data are from AG service agencies: 6 gal per operating hour per 100 HP
DataRead.basevalue = 6.0 

-- can not be 0. Use this value to adjust the fuel usage to your preference
DataRead.usemodifier = 2.0 

-- Enable DEF usage. DONT! Not until Giants enables it.
DataRead.usedef = false

function DataRead.prerequisitesPresent(specializations)
    return true;
end

function DataRead:setDefaults()
	local use = 0;
	if (DataRead.vehicle ~= nil) then
		v = DataRead.vehicle;
		if (DataRead.basevalue < 0) then
			DataRead.basevalue = 6.0
		end;
		if (DataRead.basevalue > 36) then
			DataRead.basevalue = 36
		end;
		if (DataRead.usemodifier < 0) then
			DataRead.usemodifier = 0.01
		end;
		if (DataRead.usemodifier > 10) then
			DataRead.usemodifier = 10
		end;
		-- 3.786 litres = 1 gallon
		local ubase = (3.786 * DataRead.basevalue) / 3600000
		
		if v ~= nil then	
			if (v.spec_motorized ~= nil) then
				local mpow = (v.spec_motorized.motor.peakMotorTorque * 0.85) + 0.5 -- normalized 
				if v.spec_motorized.consumersByFillType[FillType.DIESEL] ~= nil then -- belt fix
					local realuse = (ubase * mpow) -- 6 gal/hr / 100hp in ms
					use = realuse * DataRead.usemodifier;
					--v.spec_motorized.consumersByFillType[FillType.DIESEL].usage = realuse * DumpData.usemodifier
				end;
				if (DataRead.usedef == true) then
					if v.spec_motorized.consumersByFillType[FillType.DEF] ~= nil then -- belt fix
						local defuse = ((ubase / 40) * mpow) -- standard OEM 1:40 ratio
						--v.spec_motorized.consumersByFillType[FillType.DEF].usage = defuse * DumpData.usemodifier
					end;
				end;
			end;
		end;		
	end;
	return use;
end;

function DataRead:loadMap()
	g_currentMission.environment:addHourChangeListener(DataRead);
	DataRead.vehicle = nil;	
	DataRead.dumpTimer = 0;
	DataRead.fuelTimer = 0;
	DataRead.operatingTime = 0; -- current operating time - reset every hour
	DataRead.curFuelUsed = 0; -- track hourly consumption
	DataRead.totalFuelUsed = 0; -- track total consumption since beginning of operation
	DataRead.hour = (1000 * 60 * 60);
	DataRead.secondRatio = .00027; -- ratio of second to hour
	
	DataRead.requestedTracking = false;

	-- Set Header Font Info 
	DataRead.Header = "Farming Simulator/Analysis Tool";
	DataRead.fontSizeHeader = 0.040;
	DataRead.fontPosXHeader = 0.85;
	DataRead.renderYPosHeader = 0.85;
	local headerHeight, headerLines = getTextHeight(DataRead.fontSizeHeader,DataRead.Header);
	
	-- padding and x offset
	DataRead.renderXPos = 0.70; -- Data X position //  0.5 = center from screen posX
	DataRead.renderXOffset = DataRead.renderXPos + 0.1; -- Data offset X position DumpData.renderXOffset
	DataRead.linePadding = 0.008;
	
	DataRead.renderYPos = DataRead.renderYPosHeader - headerHeight - DataRead.linePadding; -- Data Y position // 0.5 = center from screen
	 -- Padding between blocks
	DataRead.fontSize = 0.02; -- Font size
	DataRead.paddingBetweenLines = DataRead.fontSize + 0.004;  -- Distance between lines	
end

function DataRead:onInputEnter(_, inputValue)
	print("Entered Vehicle");
end

function DataRead:deleteMap()
end

function DataRead:SetTracking(setActive)
	if setActive then		
		DataRead.vehicle = g_currentMission.controlledVehicle;	
		DataRead.dumpTimer = 0;
	end;		
end

function DataRead:EndTracking(endTracking)
	if endTracking then		
		DataRead.vehicle = nil;
		DataRead.requestedTracking = true;
		DataRead.operatingTime = 0;
		DataRead.fuelTimer = 0;
	end;	
end

function DataRead:keyEvent(unicode, sym, modifier, isDown)
	if Input.isKeyPressed(Input.KEY_lalt) and Input.isKeyPressed(Input.KEY_q) then
		if DataRead.vehicle ~= nil then
			g_gui:showYesNoDialog({text="Quit sending data from " .. DataRead.vehicle:getName() .. "?" , title="Farming Simulator/Analysis Tool", callback=self.EndTracking, target=self}); 
		end 
	end;
end

function DataRead:update(dt)

	if g_currentMission.controlledVehicle ~= nil then
		if DataRead.vehicle == nil and DataRead.requestedTracking == false then
			g_gui:showYesNoDialog({text="Do you want to begin sending data from " .. g_currentMission.controlledVehicle:getName() .. "?", title="F.A.S.T", callback=self.SetTracking, target=self}); 	
			DataRead.requestedTracking = true;
		end;	
	else	
		DataRead.requestedTracking = false;
	end;	
	
	if DataRead.vehicle ~= nil and DataRead.dumpTimer >= 1000 then	
		local onField = DataRead.vehicle:getIsOnField() and 1 or 0;			
		local file, err = io.open ("data.txt","w");
		if file ~= nil then
			-- file:write(DebugUtil.printTableRecursively(g_currentMission,".",0,5));
			file:write(tostring(DataRead.vehicle:getName()) .. "," .. tostring(DataRead.vehicle:getLastSpeed()) .. "," .. tostring(DataRead.totalFuelUsed) .. "," .. tostring(DataRead.curFuelUsed));
			-- DataRead.setDefaults() * 3600000) / 3.786 For GPH
			file:close();
		end;
	
		--print (tostring(DumpData.vehicle:getLastSpeed()) .. "," .. tostring(onField));
		DataRead.dumpTimer = 0;
	end;
	if DataRead.vehicle ~= nil and DataRead.fuelTimer >= 1000 then
		DataRead.usemodifier = 1.0;
		local consumptionOffset;
		if DataRead.GetTerrainType() == "Field" then
			consumptionOffset = .25;
		elseif DataRead.GetTerrainType() == "Road" then
			consumptionOffset = 0;
		else 
			consumptionOffset = .12;
		end;		 -- modify consumption based on terrain
		DataRead.usemodifier = DataRead.usemodifier + consumptionOffset;
		DataRead.curFuelUsed = DataRead.setDefaults() * g_currentMission.missionInfo.timeScale;
	    DataRead.totalFuelUsed = DataRead.totalFuelUsed + DataRead.curFuelUsed;
		print ("Terrain: " .. DataRead.GetTerrainType() .. " Offset: " .. tostring(consumptionOffset) .. "Modifier: " .. tostring(DataRead.usemodifier) .. " Total: " .. tostring(DataRead.totalFuelUsed) .. " Current: " .. tostring(DataRead.curFuelUsed) .. "GPH: " .. tostring(DataRead.setDefaults() * 3600000) / 3.786);
		DataRead.fuelTimer = 0;
	end;
	DataRead.dumpTimer = DataRead.dumpTimer + dt;
	DataRead.fuelTimer = DataRead.fuelTimer + dt;
end

function DataRead:VehicleOnRoad ()
	local onRoad = false;
	if DataRead.vehicle ~= nil then
		for _,wheel in ipairs(DataRead.vehicle:getWheels()) do
			onRoad = wheel.contact == Wheels.WHEEL_OBJ_CONTACT;
			if onRoad then
				break;
			end;
		end;
	end;
	return onRoad;
end

function DataRead:GetTerrainType()				
	local terrainType = "--";
	if DataRead.vehicle ~= nil then 
		local onRoad  = DataRead.VehicleOnRoad();
		if  DataRead.vehicle:getIsOnField() then
			terrainType = "Field";
		elseif onRoad then
			terrainType = "Road";
		else 
			terrainType = "Offroad"
		end;
	end;
	return terrainType;
end

function DataRead:draw()
	setTextColor(1,1,1,1);
	setTextBold(true);
    setTextAlignment(RenderText.ALIGN_LEFT);
	local fontHeight, fontLines;
	local yPos = DataRead.renderYPos;
	local speed, terrainType, tireCondition, fuelUsage, controlHint;
	
	if DataRead.vehicle ~= nil then
		terrainType = DataRead.GetTerrainType();
		tireCondition = "100%";
		fuelUsage =   tostring(DataRead.setDefaults() * 3600000 / 3.786 ) .. " GPH";
		controlHint = "alt + q to stop tracking vehicle";
	else 
		speed = "--";
		terrainType = "--";
		tireCondition = "--";
		fuelUsage = "--";
		controlHint = "";
	end;
     
    renderText(DataRead.fontPosXHeader, DataRead.renderYPosHeader, DataRead.fontSizeHeader, ""); -- clear text
	renderText(DataRead.fontPosXHeader, DataRead.renderYPosHeader, DataRead.fontSizeHeader, "FAST");
	
	setTextBold(false);
	
	renderText(DataRead.renderXOffset,yPos, DataRead.fontSize, "Fuel Usage .....");
	xPos = 1.0 - getTextWidth(DataRead.fontSize, fuelUsage);
	renderText(xPos, yPos, DataRead.fontSize, fuelUsage);
	
	fontHeight, fontLines = getTextHeight(DataRead.fontSize,"Fuel Usage ..... .0023 gallon/km");
	yPos =  yPos - fontHeight - DataRead.linePadding;
	
	renderText(DataRead.renderXOffset,yPos, DataRead.fontSize, "Tire Conditions .....");
	setTextColor(0,1,0,1);
	xPos = 1.0 - getTextWidth(DataRead.fontSize, tireCondition);
	renderText(xPos, yPos, DataRead.fontSize, tireCondition);
	
	fontHeight, fontLines = getTextHeight(DataRead.fontSize, "Tire Conditions ..... 100%");
	yPos =  yPos - fontHeight - DataRead.linePadding;
	
	setTextColor(1,1,1,1);
	renderText(DataRead.renderXOffset,yPos, DataRead.fontSize, "Terrain Type .....");
	xPos = 1.0 - getTextWidth(DataRead.fontSize, terrainType);
	renderText(xPos, yPos, DataRead.fontSize, terrainType);
	
	fontHeight, fontLines = getTextHeight(DataRead.fontSize, "Terrain Type ..... No");
	yPos =  yPos - fontHeight - DataRead.linePadding;
	renderText(DataRead.renderXOffset,yPos, DataRead.fontSize, controlHint);
	--sDumpData.drawText(DumpData.renderXOffset, yPos, "Fuel usage:", "1 gallon", 1, 1, 1);
end
addModEventListener(DataRead);