#include "SimplePlayer.h"

using namespace CLIPlayer;

SimplePlayer::SimplePlayer()
{
}

void SimplePlayer::Update(GameInfo^ gameInfo) {}

void SimplePlayer::Initialize(GameInfo^ gameInfo, GameSetting^ gameSetting)
{
	me = gameInfo->Agent;
}

void SimplePlayer::DayStart() {}

String^ SimplePlayer::Talk()
{
	return "Over";
}

String^ SimplePlayer::Whisper()
{
	return "Over";
}

Agent^ SimplePlayer::Vote()
{
	return me;
}

Agent^ SimplePlayer::Attack()
{
	return me;
}

Agent^ SimplePlayer::Divine()
{
	return me;
}

Agent^ SimplePlayer::Guard()
{
	return me;
}

void SimplePlayer::Finish() {}
