import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { Header } from "../../components/Header/Header";
import styles from './AuthorPage.module.css'
import { useEffect, useState } from "react";
import { backendUrl } from "../../BackendUrl";
import { client } from "../../client/client";
import { UserDto } from "../../dto/User/UserDto";
import { FilterAndSortingPanel } from "../../components/FilterAndSortingPanel/FilterAndSortingPanel";
import { ListBooks } from "../../components/ListBooks/ListBooks";
import { SortState } from "../../components/FilterAndSortingPanel/SortState";
import { SearchParams, parseUrl } from "../../hook/parseUrl";
import { useAuth } from "../../hook/useAuth";
import { NotFound } from "../../components/NotFound/NotFound";

export function AuthorPage() {    
    const [isFound, setFound] = useState<boolean>(true)
    const {id} = useParams()
    const [isAdminHere, setIsAdmin] = useState<boolean>()
    const authStore = useAuth()
    const [data, setData] = useState<UserDto>()
    const [seenAbout, setSeenAbout] = useState<boolean>(true)
    const [seenBooks, setSeenBooks] = useState<boolean>(false)   
    const [searchParams, setSearchParams] = useSearchParams()
    const [params, setParam] = useState<SearchParams>({
        sortState: SortState.Error,
        genresIds: []
    })
    
    const navigate = useNavigate()
    const clickOnProfile = () => {
        setSeenAbout(true)
        setSeenBooks(false)
    }

    const clickOnBooks = () => {
        setSeenAbout(false)
        setSeenBooks(true)
    }
    
    useEffect(() => {
        const getData = async () => {
            await client.get<UserDto>(backendUrl + 'user/get-user/' + id )
            .then((response) => {
                setData(response.data)
                console.log(response.data)
                authStore.isAuth && authStore.user.Role == 'admin' ? setIsAdmin(true) : setIsAdmin(false)
                const sortState = searchParams.get('sort-state')
                const genres = searchParams.getAll('genres-ids')
                const params = parseUrl(sortState, genres)
                console.log('params', params)
                setParam(params)
                params.sortState !== SortState.Error || params.genresIds.length != 0 ? clickOnBooks() : clickOnProfile()
            })
            .catch(error => {
                if (error.response.status === 404) {
                    // console.log(error.response.status)
                    setFound(false)
                }
            //   console.log(error.response.data)
            })
        }
        getData()
    }, [])

    const selectedButton = {
        borderBottom: '3px solid blue',
    }

    const unSelectedButton = {
        borderBottom: 'none',       
    }
    
    const deleteUser = async () => {
        await client.delete('user/delete/' + data.id)
        .then(response => {
            if (data.id === authStore.user.Id) {
                authStore.removeUser()
            }
        })
        .catch(error => {
            console.log(error.response.data)
        })
        await client.get('auth/logout/' + authStore.user.Id)
        .then(response => {
            console.log(response.data)
            // authStore.removeUser()
            // setShow(false)
            navigate('/')
        })
        .catch(error => {
            console.log(error)
        })
    }

    return(
        <>
        {isFound ? 
            <div className={styles.profile}>
                <div className={styles.profile_header}>
                    {/* <div style = {styleIcon} className={styles.profile_header_icon}> */}
                    {/* <img src={props.img}  /> */}
                    <img src={data?.photoPath} className={styles.profile_header_icon}/>                                    
                    <h2>{data?.name}</h2>
                    {isAdminHere ? <button className={styles.button_delete} 
                            onClick={deleteUser}>Уничтожить аккаунт</button>
                    : null}
                </div>
                <div className={styles.profile_content}>
                    <div className={styles.content_menu}>
                        <button style={seenAbout ? selectedButton : unSelectedButton} className={styles.content_menu_button}
                        onClick = {clickOnProfile}>                            
                            Профиль
                        </button>
                        <button style={seenBooks ? selectedButton : unSelectedButton} className={styles.content_menu_button}
                            onClick = {clickOnBooks}
                            >
                            Произведения
                        </button>
                    </div>
                    {seenAbout ? 
                    <div className={styles.content_section}>
                    <div className={styles.info}>
                            <p>Почта: {data?.email}</p>
                            <p>Дата регистрации: {data?.dateRegistration === undefined ? null : 
                            data?.dateRegistration.substring(0, 10)}</p>
                    </div>
                    <div className={styles.about}>
                        <label className={styles.label_about}>О себе:</label>
                        <textarea className={styles.textarea_about}
                                readOnly style={{outline: 'none' }}
                                defaultValue={data?.about === null ? '' : data?.about}
                        ></textarea>
                    </div>
                    </div>
                    : null}
                    {seenBooks ?
                    <>
                    <FilterAndSortingPanel sortState={params.sortState == SortState.Error ? SortState.None 
                    : params.sortState} genres={
                        params.genresIds
                    } />
                    <ListBooks genresIds={params.genresIds} sortState={params.sortState == SortState.Error ?
                     SortState.None 
                    : params.sortState} authorId={data.id} readerId={-1}/>
                    </>
                    : null}
                </div>
            </div>
            :
            <NotFound />
            }

        </>
    )
}