import clr

clr.AddReference("AIWolfLibCommon")
from AIWolf.Common.Data import *
from AIWolf.Common.Net import *

class SimplePlayer(object):
    me = -1

    def get_Name(self):
       return "SimplePlayer(IronPython)"

    def Attack(self):
        return Agent.GetAgent(self.me)

    def DayStart(self):
        pass

    def Divine(self):
        return Agent.GetAgent(self.me)

    def Finish(self):
        pass
    
    def Guard(self):
        return Agent.GetAgent(self.me)

    def Initialize(self, gameInfo, gameSetting):
        self.me = gameInfo.Agent.AgentIdx

    def Talk(self):
        return Talk.OVER

    def Update(self, gameInfo):
        pass

    def Vote(self):
        return Agent.GetAgent(self.me)

    def Whisper(self):
        return Talk.OVER