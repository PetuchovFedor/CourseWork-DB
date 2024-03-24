export interface UpdateAboutDto {
    id: number,
    about: string | null
}

export interface UpdatePhoto {
    id: number,
    file: File
}

export interface UpdatePassword {
    id: number,
    oldPassword: string,
    newPassword: string
}

export interface UpdateLks {
    id: number,
    email: string,
    name: string
}