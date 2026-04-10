using Domain.Entities;
using Domain.Interfaces;

namespace Business.Services;

public class TagService
{
    private readonly ITagRepository _repo;
    public TagService(ITagRepository repo) => _repo = repo;

    public Tag Create(string name)
    {
        var tag = new Tag(name);
        _repo.Add(tag);
        return tag;
    }

    public Tag GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Tag {id} not found.");

    public IEnumerable<Tag> GetAll() => _repo.GetAll();

    public void Update(int id, string name) => GetById(id).UpdateName(name);

    public bool Delete(int id) => _repo.Delete(id);
}
