import { useEffect, useState } from "react"
import { IconDto } from "../../dto/IconDto"
import { useSearchParams } from "react-router-dom"
import { client } from "../../client/client"
import styles from './SearchPage.module.css'
import InfiniteScroll from "react-infinite-scroll-component"
import { ImageBlock } from "../../components/ImageBlock/ImageBlock"
import { RedirectEnum } from "../../components/ImageBlock/RedirectEnum"

const style = {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
    overflow: 'visible',
    background: 'white',
    padding: '10px',
    width: '100%',
    // marginLeft: 'auto',
    // marginRight: 'auto',
    marginTop: '30px'
}

export function SearchPage() {
    const [isVisibleUsers, setVisUsers] = useState<boolean>(false)
    const [isVisibleBooks, setVisBooks] = useState<boolean>(false)
    const [countBooks, setCountBooks] = useState<number>(0)
    const [countUsers, setCountUsers] = useState<number>(0)
    const [books, setBooks] = useState<IconDto[]>([])
    const [users, setUsers] = useState<IconDto[]>([])
    const [searchParams, setSearchParams] = useSearchParams()
    const [hasMoreUsers, setHasMoreUsers] = useState<boolean>(true)
    const [scippedUsers, setScippedUsers] = useState<number>(0)
    const [hasMoreBooks, setHasMoreBooks] = useState<boolean>(true)
    const [scippedBooks, setScippedBooks] = useState<number>(0)

    useEffect(() => {
        const name = searchParams.get('name')
        const getBooks = async () => {
            // console.log('book/find-book/' +  + '/?scipped=' + scippedBooks)
            await client.get('book/find-book/' + name + '/?scipped=' + scippedBooks)
            .then(response => {
                console.log(response.data)
                setCountBooks(response.data.totalNumber)
                setBooks(response.data.books)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }

        const getUsers = async () => {
            await client.get('user/find-user/' + name + '/?scipped=' + scippedBooks)
            .then(response => {
                console.log(response.data)
                setCountUsers(response.data.totalNumber)
                setUsers(response.data.users)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
        getBooks()
        getUsers()
    }, [searchParams])

    const loadMoreUsers = async () => {
        const tempScip =  scippedUsers + 15  
        setScippedUsers(tempScip)
        if (tempScip > countUsers) {       
            setHasMoreUsers(false)
            return
        }
        await client.get('user/find-user/' + searchParams.get('name') + '/?scipped=' + tempScip)
        .then((response) => {
            if (response.data.users.length == 0) {
                setHasMoreUsers(false)
            }
            setUsers(users.concat(response.data.users))
            setCountUsers(response.totalNumber)
            console.log(data)
        })
        .catch(error => {
          console.log(error.response.data)
        })
    }

    const loadMoreBooks = async () => {
        const tempScip =  scippedBooks + 15  
        setScippedBooks(tempScip)
        if (tempScip > countBooks) {       
            setHasMoreUsers(false)
            return
        }
        await client.get('book/find-book/' + searchParams.get('name') + '/?scipped=' + tempScip)
        .then((response) => {
            if (response.data.boooks.length == 0) {
                setHasMoreBooks(false)
            }
            setBooks(books.concat(response.data.books))
            setCountBooks(response.totalNumber)
            console.log(data)
        })
        .catch(error => {
          console.log(error.response.data)
        })
    }

    return (
        <>        
        {isVisibleBooks ? 
        // countBooks !== 0 ?
        <div className={styles.panel}>
            <button className={styles.close_button} onClick={() => setVisBooks(false)}>Закрыть</button>
            {countBooks != 0 ?<InfiniteScroll
            // id={'users'}
            // className={styles.books_section}
            dataLength={books.length}
            next={loadMoreBooks}
            hasMore={hasMoreBooks}
            loader={null}
            style={style}
            >
            
            {books.map(books => {
                return <ImageBlock id={books.id} name={books.name} img={books.img} 
                width={200} height={200} redirect={RedirectEnum.book} />
            })}
            </InfiniteScroll> : null}
        </div> 
        : isVisibleUsers ?
        // countUsers !== 0 ?
        <div className={styles.panel}>
            <button className={styles.close_button} onClick={() => setVisUsers(false)}>Закрыть</button>
            {countUsers != 0 ? <InfiniteScroll
            // id={'users'}
            // className={styles.books_section}
            dataLength={users.length}
            next={loadMoreUsers}
            hasMore={hasMoreUsers}
            loader={null}
            style={style}
            >
            
            {users.map(user => {
                return <ImageBlock id={user.id} name={user.name} img={user.img} 
                width={200} height={200} redirect={RedirectEnum.user} />
            })}
            </InfiniteScroll> : null}
        </div>
        :
        <div className={styles.stats_panel}>
            <p onClick={() => setVisUsers(true)} className={styles.stats_label}>Пользователи: {countUsers}</p>
            <p onClick={() => setVisBooks(true)} className={styles.stats_label}>Произведения: {countBooks}</p>
        </div>}
        </>
    )
}