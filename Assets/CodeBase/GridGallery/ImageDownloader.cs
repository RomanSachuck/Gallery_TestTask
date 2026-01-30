using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.GridGallery
{
    public class ImageDownloader
    {
        private const string BaseURL = "http://data.ikppbb.com/test-task-unity-data/pics/";
        
        private readonly Dictionary<int, Texture2D> _cachedTextures = new ();
        private readonly ConcurrentDictionary<int, Task<Sprite>> _pendingDownloads = new();

        public async Task<bool> IsImageExists(int imageNumber)
        {
            string url = $"{BaseURL}{imageNumber}.jpg";
    
            using (UnityWebRequest request = UnityWebRequest.Head(url))
            {
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                
                while (Application.isPlaying && operation.isDone == false)
                    await Task.Yield();
        
                return request.result == UnityWebRequest.Result.Success && 
                       request.responseCode == 200;
            }
        }
        
        public async Task<Sprite> GetSpriteForNumber(int imageNumber)
        {
            if (_cachedTextures.TryGetValue(imageNumber, out Texture2D cachedTexture))
            {
                return CreateSpriteFromTexture(cachedTexture);
            }
            
            if (_pendingDownloads.TryGetValue(imageNumber, out var pendingTask))
            {
                return await pendingTask;
            }
            
            Task<Sprite> downloadTask = DownloadAndCacheImage(imageNumber);
            _pendingDownloads[imageNumber] = downloadTask;
            
            try
            {
                Sprite sprite = await downloadTask;
                return sprite;
            }
            finally
            {
                _pendingDownloads.TryRemove(imageNumber, out _);
            }
            
        }

        private async Task<Sprite> DownloadAndCacheImage(int imageNumber)
        {
            string url = $"{BaseURL}{imageNumber}.jpg";

            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();

                while (Application.isPlaying && operation.isDone == false)
                {
                    await Task.Yield();
                }
   
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    
                    _cachedTextures[imageNumber] = texture;

                    return CreateSpriteFromTexture(texture);
                }
                else
                {
                   throw new Exception($"Ошибка загрузки: {request.error}");
                }
            }
        }

        private Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
    }
}