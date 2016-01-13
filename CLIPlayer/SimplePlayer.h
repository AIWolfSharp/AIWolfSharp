#pragma once

using namespace System;
using namespace AIWolf::Common::Data;
using namespace AIWolf::Common::Net;

namespace CLIPlayer {

	public ref class SimplePlayer : public IPlayer
	{
	private:
		Agent^ me;

	public:
		SimplePlayer();

		virtual property String^ Name
		{
			String^ get() sealed
			{
				return this->GetType()->ToString();
			}
		}

		virtual void Update(GameInfo^) sealed;

		virtual void Initialize(GameInfo^, GameSetting^) sealed;

		virtual void DayStart() sealed;

		virtual String^ Talk() sealed;

		virtual String^ Whisper() sealed;

		virtual Agent^ Vote() sealed;

		virtual Agent^ Attack() sealed;

		virtual Agent^ Divine() sealed;

		virtual Agent^ Guard() sealed;

		virtual void Finish() sealed;

	};
}