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

https://user-images.githubusercontent.com/25569360/206412849-9ecab700-f025-442c-88b4-ff2814521bd5.mp4
