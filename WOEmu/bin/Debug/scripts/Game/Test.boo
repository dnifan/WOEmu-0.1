//This file demonstrates uses of the general 'Game' scripts
import WOEmu
import WO.Core.Logger

def Init():
	Logger.AppendLine("Main script loaded successfully!")
	
def OnChat(client, channel, text):
	print("${client.player.Name} says '${text}'")
	
def OnTileMenuClick(executor, target, menuid):
	if menuid == 4: //"Drink"
		executor.Water = 0
		executor.UpdateStats()

def OnRequestTileMenu(executor, tile):
	optionNums = []
	
	if (tile.Height < 0):
		optionNums.Add(4) //"Drink"
		
	optionNums.Add(1) //"Dig"
	
	return optionNums

	
	