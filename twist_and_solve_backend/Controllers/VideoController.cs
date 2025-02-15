using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using System.Collections.Generic;

namespace twist_and_solve_backend.Controllers
{
    #region Controller
    [Route("[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        #region Fields
        private readonly VideoRepository _videoRepository;
        #endregion

        #region Constructor
        public VideoController(VideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }
        #endregion

        #region Video Retrieval
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAllVideos()
        {
            List<VideoModel> videos = _videoRepository.GetAllVideos();
            if (videos.Count == 0)
            {
                return NotFound("No videos found.");
            }
            return Ok(videos);
        }

        [HttpGet("ByLessonId/{lessonId}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetVideosByLessonId(int lessonId)
        {
            List<VideoModel> videos = _videoRepository.GetVideosByLessonId(lessonId);
            if (videos.Count == 0)
            {
                return NotFound($"No videos found for lesson with ID {lessonId}.");
            }
            return Ok(videos);
        }

        [HttpGet("{videoId}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetVideoById(int videoId)
        {
            VideoModel video = _videoRepository.GetVideoById(videoId);
            if (video == null)
            {
                return NotFound($"Video with ID {videoId} not found.");
            }
            return Ok(video);
        }
        #endregion

        #region Video Management
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateVideo([FromBody] VideoModel video)
        {
            if (video == null)
            {
                return BadRequest("Invalid video data.");
            }

            bool isInserted = _videoRepository.InsertVideo(video);
            if (isInserted)
            {
                return CreatedAtAction(nameof(GetVideoById), new { videoId = video.VideoId }, video);
            }
            return BadRequest("Failed to create video.");
        }

        [HttpPut("{videoId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateVideo(int videoId, [FromBody] VideoModel video)
        {
            if (video == null || video.VideoId != videoId)
            {
                return BadRequest("Invalid video data.");
            }

            bool isUpdated = _videoRepository.UpdateVideo(video);
            if (isUpdated)
            {
                return Ok(video);
            }
            return BadRequest("Failed to update video.");
        }

        [HttpDelete("{videoId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVideo(int videoId)
        {
            bool isDeleted = _videoRepository.DeleteVideo(videoId);
            if (isDeleted)
            {
                return Ok($"Video with ID {videoId} has been deleted.");
            }
            return NotFound($"Video with ID {videoId} not found.");
        }
        #endregion
    }
    #endregion
}
