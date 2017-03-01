setAircraftType("A-10C")
faceplate = addSprite("resources/faceplate",0,0,nil,nil)
needle = addSprite("resources/needle",150,150,nil,nil)
setSpriteOrigin(needle,10,130)


function testCallback(asiNeedle,wheel)
	rotateSprite(needle,asiNeedle*340)
	--print(asiNeedle)
end


subscribeSprite("c48", "c49",testCallback)