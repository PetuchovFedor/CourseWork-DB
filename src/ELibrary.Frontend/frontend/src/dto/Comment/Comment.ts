import { IconDto } from "../IconDto";

export interface CommentDto {
    id: number,
    userId: number,
    bookId: number,
    userIcon: IconDto,
    content: string
}

export interface UpdateCommentDto {
    commentId: number,
    content: string
}