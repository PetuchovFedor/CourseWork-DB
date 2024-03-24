import { CommentDto } from "../Comment/Comment"
import { GenreDto } from "../Genre/GenreDto"
import { UserMiniDto } from "../User/UserMiniDto"

export interface BookDto {
    id: number,
    name: string,
    annotation: string,
    cover: string,
    publicationDate: Date,
    genres: GenreDto[],
    rating: number,
    authors: UserMiniDto[],
    comments: CommentDto[]
}

export interface ChangeFileDto {
    id: number | undefined, 
    file: File
}

export interface CreateBookDto {
    name: string,
    authors: string[],
    annotation: string,
    genres: number[],
    cover: File,
    bookFile: File
}

export interface EditBookDto {
    id: number | undefined,
    name: string| undefined,
    annotation: string| undefined
}

export interface SelectionBook {
    genresIds: number[],
    sortingType: number,
    scipped: number,
    authorId: number,
    readerId: number
}
