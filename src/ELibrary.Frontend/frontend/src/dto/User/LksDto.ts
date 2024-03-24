import { IconDto } from "../IconDto"

export interface LksDto {
    id: number,
    name: string,
    about: string | null,
    photoPath: string,
    email: string,
    dateRegistration: string,
    // userBooks: IconDto[],    
}