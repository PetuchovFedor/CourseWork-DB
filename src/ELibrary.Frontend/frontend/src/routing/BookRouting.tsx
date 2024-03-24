import { Route, Routes } from "react-router-dom";
import { BooksPage } from "../pages/BooksPage/BooksPage";
import { BookPage } from "../pages/BookPage/BookPage";

export function BookRouting() {
    
    return(
        <Routes>
            <Route path='/:id' element ={<BookPage />} />
            <Route path='/' element ={<BooksPage />} />
        </Routes>
    )
}