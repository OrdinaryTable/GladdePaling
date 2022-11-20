using HarmonyLib;
using IPA;
using System.Linq;
using IPALogger = IPA.Logging.Logger;

namespace GladdePaling;

[Plugin(RuntimeOptions.SingleStartInit)]
public class Plugin {
    private Harmony _harmony;
    
    [Init]
    public void Init(IPALogger logger) {
        _harmony = new Harmony("com.github.ordindarytable.gladdepaling");
    }

    [OnEnable]
    public void OnEnable() {
        _harmony.Patch(
            AccessTools.Method(typeof(LevelCollectionTableView), nameof(LevelCollectionTableView.SetData)),
            new HarmonyMethod(AccessTools.Method(typeof(Plugin), nameof(Filter)), Priority.First)
        );
    }
    
    [OnDisable]
    public void OnDisable() {
        _harmony.UnpatchSelf();
    }

    public static void Filter(ref IPreviewBeatmapLevel[] previewBeatmapLevels) {
        if (previewBeatmapLevels == null) return;
        previewBeatmapLevels = previewBeatmapLevels.ToList().Where(level => IsWorthy(in level)).ToArray();
    }

    private static bool IsWorthy(in IPreviewBeatmapLevel level) {
        return level.songAuthorName.ToLower().Contains("gladde paling") || level.songSubName.ToLower().Contains("gladde paling");
    }
}