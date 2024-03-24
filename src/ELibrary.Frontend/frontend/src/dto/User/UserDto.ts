import { IconDto } from "../IconDto";

export interface UserDto {
    id: number,
    name: string,
    email: string
    about: string | null,
    photoPath: string,
    dateRegistration: Date
}
