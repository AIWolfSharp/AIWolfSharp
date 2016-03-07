//
// 1�i�ڂ�wrapper(C++, unmanaged managed ����)
//

#include "NativeWrapper.h"
#include "org_aiwolf_sharp_NativePlayer.h"

// ���̃��b�p�[�̊֐����Ă�Java�̃N���X
const char* javaClassName = "org/aiwolf/sharp/NativePlayer";
// CSWrapper�̃C���X�^���X�ւ̃|�C���^���i�[����C��L�N���X�̃t�B�[���h��
const char* javaContextFieldName = "nativeContext";

void storePlayerInstance(JNIEnv* env, jobject obj, CSWrapper^ wrapper)
{
	// Java��NativePlayer�N���X��nativeContext�t�B�[���h������
	jclass clazz = env->FindClass(javaClassName);
	if (clazz == NULL)
	{
		return;
	}
	jfieldID nativeContextFID = env->GetFieldID(clazz, javaContextFieldName, "J");
	if (nativeContextFID == NULL)
	{
		return;
	}
	// managed�N���X�ւ̃|�C���^��64�r�b�g�����Ƃ��Ď擾��
	// �Ăяo������Java��NativePlayer�N���X��nativeContext�t�B�[���h�Ɋi�[
	env->SetLongField(obj, nativeContextFID, ((IntPtr)GCHandle::Alloc(wrapper)).ToInt64());
}

CSWrapper^ restorePlayerInstance(JNIEnv* env, jobject obj)
{
	// Java��NativePlayer�N���X��nativeContext�t�B�[���h������
	jclass clazz = env->FindClass(javaClassName);
	if (clazz == NULL)
	{
		return nullptr;
	}
	jfieldID nativeContextFID = env->GetFieldID(clazz, javaContextFieldName, "J");
	if (nativeContextFID == NULL)
	{
		return nullptr;
	}
	// �Ăяo������Java��NativePlayer�N���X��nativeContext�t�B�[���h��
	// managed�N���X�ւ̃|�C���^���C#�v���C���[�̃C���X�^���X�𓾂�
	return (CSWrapper^)GCHandle::FromIntPtr(IntPtr(env->GetLongField(obj, nativeContextFID))).Target;
}

String^ jsToS(JNIEnv* env, jobject obj, jstring js)
{
	return gcnew String(env->GetStringUTFChars(js, NULL));
}

jstring sToJs(JNIEnv* env, jobject obj, String^ s)
{
	IntPtr intPtr;
	jstring rString;
	try
	{
		intPtr = Marshal::StringToHGlobalAnsi(s);
		rString = env->NewStringUTF((char*)intPtr.ToPointer());
	}
	finally
	{
		Marshal::FreeHGlobal(intPtr);
	}
	return rString;
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    createNativeInstance
* Signature: (Ljava/lang/String;Ljava/lang/String;)V
*/
JNIEXPORT void JNICALL Java_org_aiwolf_sharp_NativePlayer_createNativeInstance
(JNIEnv* env, jobject obj, jstring dllFileName, jstring playerClassName)
{
	CSWrapper^ wrapper = gcnew CSWrapper(jsToS(env, obj, dllFileName), jsToS(env, obj, playerClassName));
	storePlayerInstance(env, obj, wrapper);
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    getNameNative
* Signature: ()Ljava/lang/String;
*/
JNIEXPORT jstring JNICALL Java_org_aiwolf_sharp_NativePlayer_getNameNative
(JNIEnv* env, jobject obj)
{
	return sToJs(env, obj, restorePlayerInstance(env, obj)->Name);
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    updateNative
* Signature: (Ljava/lang/String;)V
*/
JNIEXPORT void JNICALL Java_org_aiwolf_sharp_NativePlayer_updateNative
(JNIEnv* env, jobject obj, jstring packetString)
{
	restorePlayerInstance(env, obj)->Update(jsToS(env, obj, packetString));
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    initializeNative
* Signature: (Ljava/lang/String;)V
*/
JNIEXPORT void JNICALL Java_org_aiwolf_sharp_NativePlayer_initializeNative
(JNIEnv* env, jobject obj, jstring packetString)
{
	restorePlayerInstance(env, obj)->Initialize(jsToS(env, obj, packetString));
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    dayStartNative
* Signature: ()V
*/
JNIEXPORT void JNICALL Java_org_aiwolf_sharp_NativePlayer_dayStartNative
(JNIEnv* env, jobject obj)
{
	restorePlayerInstance(env, obj)->DayStart();
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    talkNative
* Signature: ()Ljava/lang/String;
*/
JNIEXPORT jstring JNICALL Java_org_aiwolf_sharp_NativePlayer_talkNative
(JNIEnv* env, jobject obj)
{
	return sToJs(env, obj, restorePlayerInstance(env, obj)->Talk());
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    whisperNative
* Signature: ()Ljava/lang/String;
*/
JNIEXPORT jstring JNICALL Java_org_aiwolf_sharp_NativePlayer_whisperNative
(JNIEnv* env, jobject obj)
{
	return sToJs(env, obj, restorePlayerInstance(env, obj)->Whisper());
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    voteNative
* Signature: ()I
*/
JNIEXPORT jint JNICALL Java_org_aiwolf_sharp_NativePlayer_voteNative
(JNIEnv* env, jobject obj)
{
	return restorePlayerInstance(env, obj)->Vote();
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    attackNative
* Signature: ()I
*/
JNIEXPORT jint JNICALL Java_org_aiwolf_sharp_NativePlayer_attackNative
(JNIEnv* env, jobject obj)
{
	return restorePlayerInstance(env, obj)->Attack();
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    divineNative
* Signature: ()I
*/
JNIEXPORT jint JNICALL Java_org_aiwolf_sharp_NativePlayer_divineNative
(JNIEnv* env, jobject obj)
{
	return restorePlayerInstance(env, obj)->Divine();
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    guardNative
* Signature: ()I
*/
JNIEXPORT jint JNICALL Java_org_aiwolf_sharp_NativePlayer_guardNative
(JNIEnv* env, jobject obj)
{
	return restorePlayerInstance(env, obj)->Guard();
}

/*
* Class:     org_aiwolf_sharp_NativePlayer
* Method:    finishNative
* Signature: ()V
*/
JNIEXPORT void JNICALL Java_org_aiwolf_sharp_NativePlayer_finishNative
(JNIEnv* env, jobject obj)
{
	restorePlayerInstance(env, obj)->Finish();
}
