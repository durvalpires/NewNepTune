#import <AVFoundation/AVFoundation.h>
#import <UnityFramework/UnityFramework.h>  // or <UnityAppController.h> on older templates

@interface TTSManager : NSObject <AVSpeechSynthesizerDelegate, NSSpeechSynthesizerDelegate>
@property (nonatomic) AVSpeechSynthesizer* iosSynth;
@property (nonatomic) NSSpeechSynthesizer* macSynth;
@end

@implementation TTSManager

+ (instancetype)shared {
    static TTSManager* s;
    static dispatch_once_t once;
    dispatch_once(&once, ^{
        s = [TTSManager new];
        s.iosSynth = [AVSpeechSynthesizer new];
        s.iosSynth.delegate = s;
        s.macSynth = [[NSSpeechSynthesizer alloc] initWithVoice:nil];
        s.macSynth.delegate = s;
    });
    return s;
}

- (void)speak:(NSString*)text {
#if TARGET_OS_IPHONE
    AVSpeechUtterance* utt = [AVSpeechUtterance speechUtteranceWithString:text];
    [self.iosSynth speakUtterance:utt];
#else
    [self.macSynth startSpeakingString:text];
#endif
}

// iOS callback
- (void)speechSynthesizer:(AVSpeechSynthesizer*)synth
    didFinishSpeechUtterance:(AVSpeechUtterance*)utterance
{
    UnitySendMessage("PostureRuleApplier","OnTtsDone","");
}

// macOS callback
- (void)speechSynthesizer:(NSSpeechSynthesizer*)sender
         didFinishSpeaking:(BOOL)finishedSpeaking
{
    UnitySendMessage("PostureRuleApplier","OnTtsDone","");
}

@end

// C bridge function called from Unity:
extern "C" {
    void _SpeakTTS(const char* cstr) {
        NSString* str = [NSString stringWithUTF8String:cstr];
        [[TTSManager shared] speak:str];
    }
}
