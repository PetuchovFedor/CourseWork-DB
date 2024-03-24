import { Route, Routes } from "react-router-dom";
import { GenresPage } from "../pages/GenresPage/GenresPage";

export function GenreRouting() {
    return(
        <Routes>
            <Route path='/all' element={<GenresPage />} />
        </Routes>
    )
}