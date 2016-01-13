Imports AIWolf.Common.Data
Imports AIWolf.Common.Net

Namespace AIWolf.VBPlayer

    Public Class SimplePlayer : Implements IPlayer

        Private agent As Agent

        Public Sub New()

        End Sub


        Public ReadOnly Property Name As String Implements IPlayer.Name
            Get
                Return [GetType].ToString
            End Get
        End Property

        Public Sub DayStart() Implements IPlayer.DayStart
        End Sub

        Public Sub Finish() Implements IPlayer.Finish
        End Sub

        Public Sub Initialize(gameInfo As GameInfo, gameSetting As GameSetting) Implements IPlayer.Initialize
            agent = gameInfo.Agent
        End Sub

        Public Sub Update(gameInfo As GameInfo) Implements IPlayer.Update
        End Sub

        Public Function Attack() As Agent Implements IPlayer.Attack
            Return agent
        End Function

        Public Function Divine() As Agent Implements IPlayer.Divine
            Return agent
        End Function

        Public Function Guard() As Agent Implements IPlayer.Guard
            Return agent
        End Function

        Public Function Talk() As String Implements IPlayer.Talk
            Return "Over"
        End Function

        Public Function Vote() As Agent Implements IPlayer.Vote
            Return agent
        End Function

        Public Function Whisper() As String Implements IPlayer.Whisper
            Return "Over"
        End Function
    End Class
End Namespace
