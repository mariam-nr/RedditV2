using Reddit.Models;
using System.ComponentModel.DataAnnotations;

namespace Reddit.Dtos
{
    public class PostDto
    {
        [Required]
        [StringLength(20, ErrorMessage = "length must be between 5 and 20.", MinimumLength = 5)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Content { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string CommunityName { get; set; }

        public Post CreatePost() {
        return new Post { Title = Title, Content = Content,
            AuthorId = AuthorId, CommunityName = CommunityName };
        }
    }
}
