faceplate = addSprite("resources/faceplate",0,0,nil,nil)
needle = addSprite("resources/needle",150,150,nil,nil)
setSpriteOrigin(needle,10,130)


function testCallback(test)
	print(test)
end


subscribeSprite("Yo Mamma is a snow blower",testCallback)