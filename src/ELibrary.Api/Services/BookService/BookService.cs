using ELibrary.src.ELibrary.Api.Dto;
using ELibrary.src.ELibrary.Api.Services.BookFileService;
using ELibrary.src.ELibrary.Api.Services.ImageService;
using ELibrary.src.ELibrary.Domain.BookModel;
using ELibrary.src.ELibrary.Domain.CommentModel;
using ELibrary.src.ELibrary.Domain.RatingModel;
using ELibrary.src.ELibrary.Infrastructure.UoW;

namespace ELibrary.src.ELibrary.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookFileService _bookFileService;
        private readonly IImageService _imageService;

        public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, 
            IRatingRepository ratingRepository,
            ICommentRepository commentRepository, IBookFileService bookFileService,
            IImageService imageService)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
            _ratingRepository = ratingRepository;
            _commentRepository = commentRepository;
            _bookFileService = bookFileService;
            _imageService = imageService;
        }

        public async Task ChangeBookFile(ChangeFileDto bookFile)
        {
            var book = await _bookRepository.GetById(bookFile.Id);
            if (book == null)
            {
                throw new Exception();
            }
            _bookFileService.DeleteBookFile(book.DownloadUrl);
            var newFile = await _bookFileService.AddBookFile(bookFile.File);
            book.DownloadUrl = newFile;
            _bookRepository.Update(book);
            _unitOfWork.Commit();
        }

        public async Task ChangeCover(ChangeFileDto cover)
        {
            var book = await _bookRepository.GetById(cover.Id);
            if (book == null)
            {
                throw new Exception();
            }
            _imageService.DeleteImage(book.Cover);
            var newCover = await _imageService.AddImage(cover.File);
            book.Cover = newCover;
            _bookRepository.Update(book);
            _unitOfWork.Commit();
        }

        public async Task ChangeDateById(int bookId)
        {
            await _bookRepository.ChangeDateById(bookId);
            _unitOfWork.Commit();
        }

        public async Task ChangeGenresBook(ChangeGenresBookDto dto)
        {
            await _bookRepository.ChangeGenresBook(dto.BookId, dto.GenresId);
            _unitOfWork.Commit();
        }

        public async Task<int> CreateBook(CreateBookDto book)
        {
            string coverName = await _imageService.AddImage(book.Cover);
            string bookFileName = await _bookFileService.AddBookFile(book.BookFile);
            var newBook = new Book(book.Name, DateTime.Now, coverName,
                book.Annotation, bookFileName);
            var b = await _bookRepository.Create(newBook, book.Authors, book.Genres);               
            _unitOfWork.Commit();
            return b.Id;
        }

        public async Task DeleteBook(int id)
        {
            Book book = await _bookRepository.GetById(id);
            if (book == null) 
            {
                throw new Exception();
            }
            _bookFileService.DeleteBookFile(book.DownloadUrl);
            _imageService.DeleteImage(book.Cover);
            _bookRepository.Delete(book);
            _unitOfWork.Commit();
        }

        public async Task EditBook(EditBookDto book)
        {
            var b = await _bookRepository.GetById(book.Id);
            if (b == null) 
            {
                throw new Exception();
            }
            b.Name = book.Name;
            b.Annotation = book.Annotation;
            _bookRepository.Update(b);
            _unitOfWork.Commit();
        }

        public async Task<List<BookIconDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetBookFilePath(int id)
        {
            var book = await _bookRepository.GetById(id);
            return _bookFileService.GetBookFilePath(book.DownloadUrl);
        }

        public async Task<BookIconDto> GetBookIconById(int id)
        {
            var b = await _bookRepository.GetById(id);
            return new BookIconDto(b.Id, b.Name, await _imageService
                .GetImage(b.Cover));
        }

        public async Task<SelectionResultDto> GetBookIconByName(string name, int scipped)
        {
            var findResult = await _bookRepository.GetByName(name, scipped);
            var booksTask = findResult.Books.ConvertAll(async b => new BookIconDto(
                b.Id, b.Name, await _imageService.GetImage(b.Cover)));
            var books = await Task.WhenAll(booksTask);
            return new SelectionResultDto(books.ToList(), findResult.TotalNumber);
        }

        public async Task<BookDto> GetById(int id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                throw new NullReferenceException();
            }
            var avgR = await _ratingRepository.GetAvgRatingBookById(id);
            var comments = await _commentRepository.GetPartByBookId(id, 0);
            var authors = await _bookRepository.GetAuthorsByBookId(id);
            var genres = await _bookRepository.GetBooksGenres(id);
            var authorsDto = authors.ConvertAll(a => new UserMiniDto(a.Id, a.Name));
            var genresDto = genres.ConvertAll(g => new GenreDto(g.Id, g.Name));
            var taskCommentDto = comments.ConvertAll(async c => new CommentDto(c.Id, c.UserId,
                c.BookId, new UserIconDto(c.IdUserNavigation.Id, c.IdUserNavigation.Name,
                await _imageService.GetImage(c.IdUserNavigation.PhotoUser)), c.Content));
            var commentDto = await Task.WhenAll(taskCommentDto.ToArray());
            var cover = await _imageService.GetImage(book.Cover);
            return new BookDto(book.Id, book.Name, book.Annotation, cover,
                book.PublicationDate, genresDto, avgR, authorsDto, commentDto.ToList());
        }

        public async Task<SelectionResultDto> GetSelectionBook(int[] genresIds, 
            SortState sortState, int scipped, int authorId, int readerId)
        {
            var selectionResult = await _bookRepository.GetSelectionBook(genresIds, sortState,
                scipped, authorId, readerId);
            var temp = selectionResult.Books.ConvertAll(async b => new BookIconDto(b.Id, b.Name,
                await _imageService.GetImage(b.Cover)));
            var result = await Task.WhenAll(temp.ToArray());
            if (result == null)
            {
                return new SelectionResultDto(new List<BookIconDto>(),
                    selectionResult.TotalNumber);
            }
            return new SelectionResultDto(result.ToList(), selectionResult.TotalNumber);
        }
    }
}
