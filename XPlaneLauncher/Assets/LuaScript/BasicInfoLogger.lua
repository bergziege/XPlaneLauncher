dataref("TailNumber", "sim/aircraft/view/acf_tailnum", "writable")
dataref("ICAO", "sim/aircraft/view/acf_ICAO", "writable")
dataref("LiveryIndex", "sim/aircraft/view/acf_livery_index", "writable")
dataref("LiveryPath", "sim/aircraft/view/acf_livery_path", "writable")
dataref("Heading", "sim/flightmodel/position/true_psi", "writable")

last_report = os.clock()

function log_basic_infos()
    if os.clock() - last_report > 60 then
        local indexOfLastSeparator = string.find(LiveryPath, "/[^/]*$")
        local cleanedLiveryPath = ""
        if indexOfLastSeparator == nil then
            cleanedLiveryPath = LiveryPath
        else
            cleanedLiveryPath = string.sub(LiveryPath, 1, indexOfLastSeparator-1)
            cleanedLiveryPath = cleanedLiveryPath:gsub("Extra Aircraft/","")
            cleanedLiveryPath = cleanedLiveryPath:gsub("Aircraft/","")
            cleanedLiveryPath = cleanedLiveryPath:gsub("Helicopters/","")
            cleanedLiveryPath = cleanedLiveryPath:gsub("/liveries/","_")
            cleanedLiveryPath = cleanedLiveryPath:gsub(" ","-")
            cleanedLiveryPath = cleanedLiveryPath:gsub("/","_")
        end

        local cleanedAircraftFileName = AIRCRAFT_FILENAME:gsub(".acf","")

        local positionFileName = cleanedAircraftFileName .. "_" .. cleanedLiveryPath .. ".txt"
        local file = io.open("new:####|old:Z:/Bernd/X-Plane/Logs/"..positionFileName,"w")
        io.output(file)

        local payload = '{"Latitude":'..LATITUDE..',"Longitude":'..LONGITUDE..',"Elevation":'..ELEVATION..',"Heading":'.. Heading ..',"AircraftFile":"'.. AIRCRAFT_FILENAME ..'","Livery":"'.. string.sub(LiveryPath, 1, indexOfLastSeparator) ..'"}'
        io.write(payload)

        io.close(file)

        local situationFileName = cleanedAircraftFileName .. "_" .. cleanedLiveryPath .. ".sit"
        local situationPath = SYSTEM_DIRECTORY .. "Output/situations/" .. situationFileName
        save_situation(situationPath)

        last_report = os.clock()
    end
end

do_sometimes("log_basic_infos()")