#include "TwoPole_Chameleon3_Aisound3.h"

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, "TwoPole_Chameleon3_Aisound3", __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, "TwoPole_Chameleon3_Aisound3", __VA_ARGS__))

extern "C" {
	/*�˼򵥺�������ƽ̨ ABI���˶�̬���ؿ�Ϊ��ƽ̨ ABI ���б��롣*/
	const char * TwoPole_Chameleon3_Aisound3::getPlatformABI()
	{
	#if defined(__arm__)
	#if defined(__ARM_ARCH_7A__)
	#if defined(__ARM_NEON__)
		#define ABI "armeabi-v7a/NEON"
	#else
		#define ABI "armeabi-v7a"
	#endif
	#else
		#define ABI "armeabi"
	#endif
	#elif defined(__i386__)
		#define ABI "x86"
	#else
		#define ABI "unknown"
	#endif
		LOGI("This dynamic shared library is compiled with ABI: %s", ABI);
		return "This native library is compiled with ABI: %s" ABI ".";
	}

	void TwoPole_Chameleon3_Aisound3()
	{
	}

	TwoPole_Chameleon3_Aisound3::TwoPole_Chameleon3_Aisound3()
	{
	}

	TwoPole_Chameleon3_Aisound3::~TwoPole_Chameleon3_Aisound3()
	{
	}
}
