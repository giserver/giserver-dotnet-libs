namespace Giserver.CAD.Converter;

public interface ICADConverter
{
    Task DwgToDxfAsync(string dwgFile, string dxfFile, bool binary = false, CancellationToken cancellationToken = default);

    Task DwgToDxfAsync(string dwgFile, Stream dxfStream, bool binary = false, CancellationToken cancellationToken = default);

    Task DwgToDxfAsync(Stream dwgStream, string dxfFile, bool binary = false, CancellationToken cancellationToken = default);

    Task DwgToDxfAsync(Stream dwgStream, Stream dxfStream, bool binary = false, CancellationToken cancellationToken = default);
}