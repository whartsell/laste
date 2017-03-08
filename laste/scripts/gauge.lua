setAircraftType("A-10C")

--horizon = addSprite("resources/Horizon.png",56,56,338,338)
--setSpriteViewPort(horizon,0,520,338,338)
bezel = addSprite("resources/faceplate.png",0,0,300,300)
needle = addSprite("resources/needle.png",150,150)
setSpriteOrigin(needle,10,130)


function testCallback(asiNeedle,wheel)
	rotateSprite(needle,asiNeedle*340)
	print(asiNeedle)
end


subscribeSprite("c48", "c49",testCallback)