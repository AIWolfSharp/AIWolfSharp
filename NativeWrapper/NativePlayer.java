//
// NativePlayer.java
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

/**
 * JNIを利用してJavaからC#版のプレイヤーを直接実行するプレイヤークラス
 */
package org.aiwolf.sharp;

import org.aiwolf.common.data.Agent;
import org.aiwolf.common.data.Player;
import org.aiwolf.common.data.Talk;
import org.aiwolf.common.data.Vote;
import org.aiwolf.common.net.DataConverter;
import org.aiwolf.common.net.GameInfo;
import org.aiwolf.common.net.GameInfoToSend;
import org.aiwolf.common.net.GameSetting;
import org.aiwolf.common.net.JudgeToSend;
import org.aiwolf.common.net.Packet;
import org.aiwolf.common.net.TalkToSend;
import org.aiwolf.common.net.VoteToSend;

/**
 * @author otsuki
 *
 */
public class NativePlayer implements Player {

	static {
		// System.loadLibrary("NativeWrapper");
		System.load("C:/Somewhere/NativeWrapper.dll");
	}

	// C#プレイヤーのdllファイル名，C#側のNativePlayerに渡される
	private String dllFileName = "C:\\Some\\Where\\TestPlayer.dll";
	// C#プレイヤーのクラス名，C#側のNativePlayerに渡される
	private String playerClassName = "AIWolf.TestPlayer.TestRoleAssignPlayer";

	// Store for pointer to native class.
	private long nativeContext;

	// Native methods.
	private native void createNativeInstance(String dllFileName, String playerClassName);

	private native String getNameNative();

	private native void updateNative(String packetString);

	private native void initializeNative(String packetString);

	private native void dayStartNative();

	private native String talkNative();

	private native String whisperNative();

	private native int voteNative();

	private native int attackNative();

	private native int divineNative();

	private native int guardNative();

	private native void finishNative();

	// GameInfoをJSONにしてNative側に渡す際に必要
	private GameInfoToSend gameInfoToGameInfoToSend(GameInfo gameInfo) {
		GameInfoToSend gis = new GameInfoToSend();
		gis.setDay(gameInfo.getDay());
		if (gameInfo.getAgent() != null) {
			gis.setAgent(gameInfo.getAgent().getAgentIdx());
		}
		if (gameInfo.getMediumResult() != null) {
			gis.setMediumResult(new JudgeToSend(gameInfo.getMediumResult()));
		}
		if (gameInfo.getDivineResult() != null) {
			gis.setDivineResult(new JudgeToSend(gameInfo.getDivineResult()));
		}
		if (gameInfo.getExecutedAgent() != null) {
			gis.setExecutedAgent(gameInfo.getExecutedAgent().getAgentIdx());
		}
		if (gameInfo.getAttackedAgent() != null) {
			gis.setAttackedAgent(gameInfo.getAttackedAgent().getAgentIdx());
		}
		if (gameInfo.getGuardedAgent() != null) {
			gis.setGuardedAgent(gameInfo.getGuardedAgent().getAgentIdx());
		}
		for (Vote vote : gameInfo.getVoteList()) {
			gis.getVoteList().add(new VoteToSend(vote));
		}
		for (Vote vote : gameInfo.getAttackVoteList()) {
			gis.getAttackVoteList().add(new VoteToSend(vote));
		}
		for (Talk talk : gameInfo.getTalkList()) {
			gis.getTalkList().add(new TalkToSend(talk));
		}
		for (Talk talk : gameInfo.getWhisperList()) {
			gis.getWhisperList().add(new TalkToSend(talk));
		}
		for (Agent agent : gameInfo.getStatusMap().keySet()) {
			gis.getStatusMap().put(agent.getAgentIdx(), gameInfo.getStatusMap().get(agent).toString());
		}
		for (Agent agent : gameInfo.getRoleMap().keySet()) {
			gis.getRoleMap().put(agent.getAgentIdx(), gameInfo.getRoleMap().get(agent).toString());
		}
		return gis;
	}

	public NativePlayer() {
		createNativeInstance(dllFileName, playerClassName);
	}

	@Override
	public String getName() {
		return getNameNative();
	}

	@Override
	public void update(GameInfo gameInfo) {
		Packet packet = new Packet(null, gameInfoToGameInfoToSend(gameInfo));
		updateNative(DataConverter.getInstance().convert(packet));
	}

	@Override
	public void initialize(GameInfo gameInfo, GameSetting gameSetting) {
		Packet packet = new Packet(null, gameInfoToGameInfoToSend(gameInfo), gameSetting);
		initializeNative(DataConverter.getInstance().convert(packet));
	}

	@Override
	public void dayStart() {
		dayStartNative();
	}

	@Override
	public String talk() {
		return talkNative();
	}

	@Override
	public String whisper() {
		return whisperNative();
	}

	@Override
	public Agent vote() {
		return Agent.getAgent(voteNative());
	}

	@Override
	public Agent attack() {
		return Agent.getAgent(attackNative());
	}

	@Override
	public Agent divine() {
		return Agent.getAgent(divineNative());
	}

	@Override
	public Agent guard() {
		return Agent.getAgent(guardNative());
	}

	@Override
	public void finish() {
		finishNative();
	}
}
