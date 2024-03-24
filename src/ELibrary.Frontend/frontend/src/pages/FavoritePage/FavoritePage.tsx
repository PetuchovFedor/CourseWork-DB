import { useEffect } from 'react'
import { useAuth } from '../../hook/useAuth'
import styles from './FavoritePage.module.css'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { parseUrl } from '../../hook/parseUrl'
import { FilterAndSortingPanel } from '../../components/FilterAndSortingPanel/FilterAndSortingPanel'
import { ListBooks } from '../../components/ListBooks/ListBooks'
import { SortState } from '../../components/FilterAndSortingPanel/SortState'

export function FavoritePage() {
    const authStore = useAuth()
    const navigate = useNavigate()
    const [searchParams, setSearchParams] = useSearchParams()
    // const [params, setParams] = useState<SearchParams>({
    //     sortState: SortState.Error,
    //     genresIds: []
    // })
    const params = parseUrl(searchParams.get('sort-state'), searchParams.getAll('genres-ids'))
    useEffect(() => {
        if (!authStore.isAuth) {
            navigate('/')
        }
    })
    return (
        <div className={styles.page}>
            <FilterAndSortingPanel sortState={params.sortState == SortState.Error ? SortState.None 
                : params.sortState} genres={
                params.genresIds} />
            <ListBooks genresIds={params.genresIds} sortState={params.sortState == SortState.Error ?
                SortState.None 
                : params.sortState} authorId={-1} readerId={authStore.user.Id}/>
    </div>
    )
}