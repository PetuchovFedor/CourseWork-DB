import { useCallback, useEffect, useState } from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import styles from './BooksPage.module.css'
import { SearchParams, parseUrl } from "../../hook/parseUrl"
import { SortState } from "../../components/FilterAndSortingPanel/SortState"
import { FilterAndSortingPanel } from "../../components/FilterAndSortingPanel/FilterAndSortingPanel"
import { ListBooks } from "../../components/ListBooks/ListBooks"

interface IBooksPageProps {
    param: SearchParams
}

export function BooksPage() {
    // const navigate = useNavigate()
    const [searchParams, setSearchParams] = useSearchParams()
    // const [params, setParams] = useState<SearchParams>({
    //     sortState: SortState.Error,
    //     genresIds: []
    // })
    const params = parseUrl(searchParams.get('sort-state'), searchParams.getAll('genres-ids'))
    console.log(params)
    // const [, updateState] = useState();
    // const forceUpdate = useCallback(() => updateState({}), []);

    // useEffect(() => {
    //     const getParams = () => {
    //         const sortState = searchParams.get('sort-state')
    //         const genres = searchParams.getAll('genres-ids')
    //         const tempParams = parseUrl(sortState, genres)
    //         console.log('params', tempParams)
    //         setParams({...tempParams})
    //         // forceUpdate()
    //         console.log(params)
    //     }
    //     getParams()
    // }, [])

    return(
        <div className={styles.page}>
            <FilterAndSortingPanel sortState={params.sortState == SortState.Error ? SortState.None 
                : params.sortState} genres={
                params.genresIds} />
            <ListBooks genresIds={params.genresIds} sortState={params.sortState == SortState.Error ?
                SortState.None 
                : params.sortState} authorId={-1} readerId={-1}/>
        </div>
    )
}