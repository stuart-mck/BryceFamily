using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Model;

namespace BryceFamily.Repo.Core.Read.Story
{
    public interface IStoryReadRepository
    {
        Task<List<StoryIndex>> GetStoryIndexes(CancellationToken cancellationToken);

        Task<List<StoryContent>> GetStories(CancellationToken cancellationToken);

        Task<StoryContent> Load(Guid id, CancellationToken cancellationToken);
    }
}