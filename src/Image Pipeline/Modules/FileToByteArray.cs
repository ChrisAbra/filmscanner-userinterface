using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
public class FileToByteArray : ICancellable
{


    public CancellationTokenSource cancellationTokenSource {get; private set;}

    public async Task<byte[]> ReadFileAsync(String filePath){
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;

        byte[] byteArray = await File.ReadAllBytesAsync(filePath,token);

        cancellationTokenSource.Dispose();
        return byteArray;
    }

}