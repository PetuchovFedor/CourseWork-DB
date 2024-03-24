import { Route, Routes } from "react-router-dom";
import { AuthorsPage } from "../pages/AuthorsPage/AuthorsPage";
import { AuthorPage } from "../pages/AuthorPage/AuthorPage";
import { FavoritePage } from "../pages/FavoritePage/FavoritePage";


export function UserRouting() {
    return(
        <Routes>
            <Route path='/:id' element ={<AuthorPage />} />
            <Route path='/authors' element={<AuthorsPage />} />
            <Route path='/favorite' element={<FavoritePage />} />
        </Routes>
    )
}