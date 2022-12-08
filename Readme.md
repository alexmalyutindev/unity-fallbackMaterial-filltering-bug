# Unity materialFallback bug

Materials without `_fallbackTestId` pass don't get into render, till manually moved in scene!
```csharp
ShaderTagId _fallbackTestId = new ShaderTagId("FallbackTest");
...
var drawingSettings = CreateDrawingSettings(
    _fallbackTestId,
    ref renderingData,
    SortingCriteria.CommonOpaque
);

drawingSettings.fallbackMaterial = _fallbackMaterial;
context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
```

![loading preview...](Preview/fallback-test-720p.gif)