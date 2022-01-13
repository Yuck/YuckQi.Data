# Getting Started
Here's a quick start guide for working with the `YuckQi.Data` [NuGet package](https://www.nuget.org/packages/YuckQi.Data).

## Domain Models
Begin with a domain model for some entity your application will work with:
<pre><code>public class Cat : EntityBase&lt;Int64&gt;, ICreated
{
    public String? FurColor { get; set; }
    public String? Name { get; set; }
    public Decimal WeightLbs { get; set; }
    public DateTime CreationMomentUtc { get; set; }
}</code></pre>

_\* `EntityBase<T>` and `ICreated` both come from [YuckQi.Domain](https://github.com/Yuck/YuckQi.Domain)._

## Repositories
Create a repository interface for working with `Cat` entities that supports only the data operations our domain requires. Repositories should be specific to a single domain entity.
<pre><code>public interface ICatRepository
{
    Task&lt;Cat&gt; CreateCatAsync(Cat entity);
    Task&lt;Cat&gt; GetCatAsync(Int64 key);
}</code></pre>

The implementation for this repository will take abstract **data handlers** from YuckQi.Data as constructor parameters. The `UnitOfWork<T>` is provided to repositories, not services (see below), which allows applications to decide if repositories will have the same UOW scope or not.
<pre><code>public class CatRepository&lt;TScope&gt; : ICatRepository
{
    private readonly ICreationHandler&lt;Cat, Int64, TScope&gt; _creator;
    private readonly IRetrievalHandler&lt;Cat, Int64, TScope&gt; _retriever;
    private readonly IUnitOfWork&lt;TScope&gt; _uow;

    public CatRepository(IUnitOfWork&lt;TScope&gt; uow,
                         ICreationHandler&lt;Cat, Int64, TScope&gt; creator,
                         IRetrievalHandler&lt;Cat, Int64, TScope&gt; retriever)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _creator = creator ?? throw new ArgumentNullException(nameof(creator));
        _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
    }

    public Task&lt;Cat&gt; CreateCatAsync(Cat entity) => _creator.CreateAsync(entity, _uow.Scope);
    public Task&lt;Cat&gt; GetCatAsync(Int64 key) => _retriever.GetAsync(key, _uow.Scope);
}</code></pre>

## Domain Services
Repositories are used to compose domain services which span multiple related areas. The `Dog` entity is not defined here, but is being used to show how several repositories can be used together.
<pre><code>public interface IAnimalService
{
    Task&lt;Result&lt;Cat&gt;&gt; CreateCatAsync(Cat entity);
    Task&lt;Result&lt;Dog&gt;&gt; CreateDogAsync(Dog entity);
    Task&lt;Result&lt;Cat&gt;&gt; GetCatAsync(Int64 key);
    Task&lt;Result&lt;Dog&gt;&gt; GetDogAsync(Int64 key);
}

public class AnimalService : IAnimalService
{
    private readonly ICatRepository _cats;
    private readonly IDogRepository _dogs;

    public AnimalService(ICatRepository cats, IDogRepository dogs)
    {
        _cats = cats ?? throw new ArgumentNullException(nameof(cats));
        _dogs = dogs ?? throw new ArgumentNullException(nameof(dogs));
    }

    public async Task&lt;Result&lt;Cat&gt;&gt; CreateCatAsync(Cat entity) => new(await _cats.CreateCatAsync(entity));
    public async Task&lt;Result&lt;Dog&gt;&gt; CreateDogAsync(Dog entity) => new(await _dogs.CreateDogAsync(entity));
    public async Task&lt;Result&lt;Cat&gt;&gt; GetCatAsync(Int64 key) => new(await _cats.GetCatAsync(key));
    public async Task&lt;Result&lt;Dog&gt;&gt; GetDogAsync(Int64 key) => new(await _dogs.GetDogAsync(key));
}</code></pre>

_\* `Result<T>` comes from [YuckQi.Domain.Validation](https://github.com/Yuck/YuckQi.Domain)._
