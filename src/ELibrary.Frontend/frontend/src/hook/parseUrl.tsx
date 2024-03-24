import { useLocation, useSearchParams } from "react-router-dom";
import { SortState } from "../components/FilterAndSortingPanel/SortState";

export interface SearchParams {
    sortState: SortState,
    genresIds: number[]
}
export function parseUrl(sortingState: string | null, genreIds: string[]): SearchParams{
    // const [searchParams, setSearchParams] = useSearchParams()
    // const [result,]
    let tempSortState: string | null = sortingState
    let sortState: SortState = SortState.Error
    if (tempSortState !== null) {
        if (Number(tempSortState) < 6) {
            sortState = Number(tempSortState) as SortState
        }
    }
    let genresIds: number[] = genreIds.map(param => {
        return Number(param)
    })
    return {
        sortState: sortState,
        genresIds: genresIds
    }
    // console.log(searchParams.get('sort-state'), genresIds)
}