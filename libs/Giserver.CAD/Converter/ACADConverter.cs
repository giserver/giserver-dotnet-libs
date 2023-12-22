namespace Giserver.CAD.Converter;

public class ACADConverter : ICADConverter
{
    public Task DwgToDxfAsync(string dwgFile, string dxfFile, bool binary = false, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var reader = new DwgReader(dwgFile);
            var doc = reader.Read();
            using var writer = new DxfWriter(dxfFile, doc, binary);
            writer.Write();
        }, cancellationToken);
    }

    public Task DwgToDxfAsync(string dwgFile, Stream dxfStream, bool binary = false, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var reader = new DwgReader(dwgFile);
            var doc = reader.Read();
            using var writer = new DxfWriter(dxfStream, doc, binary);
            writer.Write();
        }, cancellationToken);
    }

    public Task DwgToDxfAsync(Stream dwgStream, string dxfFile, bool binary = false, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var reader = new DwgReader(dwgStream);
            var doc = reader.Read();
            using var writer = new DxfWriter(dxfFile, doc, binary);
            writer.Write();
        }, cancellationToken);
    }

    public Task DwgToDxfAsync(Stream dwgStream, Stream dxfStream, bool binary = false, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var reader = new DwgReader(dwgStream);
            var doc = reader.Read();
            using var writer = new DxfWriter(dxfStream, doc, binary);
            writer.Write();
        }, cancellationToken);
    }
}