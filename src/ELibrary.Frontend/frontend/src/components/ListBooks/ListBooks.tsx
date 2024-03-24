import { useEffect, useState } from "react"
import { IconDto } from "../../dto/IconDto"
import { SortState } from "../FilterAndSortingPanel/SortState"
import { ImageBlock } from "../ImageBlock/ImageBlock"
import { RedirectEnum } from "../ImageBlock/RedirectEnum"
import styles from './ListBooks.module.css'
import InfiniteScroll from "react-infinite-scroll-component"
import { client } from "../../client/client"
import { SelectionBook } from "../../dto/Book/BookDto"

interface IListBooksProps {
    // userBooks: IconDto[] 
    genresIds: Array<number>,    
    sortState: SortState,
    authorId: number,
    readerId: number
}

export function ListBooks(props: IListBooksProps) {
    const [countBooks, setCountBooks] = useState<number>(0)
    const [hasMore, setHasMore] = useState<boolean>(true)
    const [scipped, setScipped] = useState<number>(0)
    const [books, setBooks] = useState<IconDto[]>([])
    // const imageBlockList = props.userBooks.map(book => {
    //     console.log(book.img)
    //     return <ImageBlock id={book.id} name={book.name} img={book.img} width={200} height={200} 
    //     redirect={RedirectEnum.book}/>
    // })
    const loadData = async (scip: number) => {
        const dto: SelectionBook = {
            ...props,
            sortingType: props.sortState as number,
            scipped: scip
        }
        console.log(dto)
        await client.post<IconDto[]>('book/get-selection-books', dto)
        .then(response => {
            if (books != response.data.books) {
                setBooks(books.concat(response.data.books))
            }
            if (response.data.books.length == 0) {
                setHasMore(false)
            }
            setCountBooks(response.data.totalNumber)
            console.log(response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    useEffect(() => {
        const getData = async () => {
            const dto: SelectionBook = {
                ...props,
                sortingType: props.sortState as number,
                scipped: scipped
            }
            console.log(dto)
            await client.post<IconDto[]>('book/get-selection-books', dto)
            .then(response => {
                if (books != response.data.books) {
                    setBooks(response.data.books)
                }
                if (response.data.books.length == 0) {
                    setHasMore(false)
                }
                setCountBooks(response.data.totalNumber)
                console.log(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
        // let elem = document.querySelector('infinite-scroll-component__outerdiv')
        // elem.style.height = '100%'
        // elem.style.background = 'white'
        console.log(props)
        getData()
    }, [])

    const style = {
        display: 'flex',
        flexDirection: 'row',
        flexWrap: 'wrap',
        overflow: 'visible',
        background: 'white',
        padding: '10px'
    }

    const loadMore = async () => {
       const tempScip: number =  scipped + 15      
       setScipped(tempScip)
       if (tempScip > countBooks) {       
            setHasMore(false)
            return
       }
       await loadData(tempScip)
    }

    return(
        <> {books.length == 0 ? null :
            // <div className={styles.books_section}>
            <InfiniteScroll
            // className={styles.books_section}
            dataLength={books.length}
            next={loadMore}
            hasMore={hasMore}
            loader={null}
            style={style}>
            
            {books.map(book => {
                return <ImageBlock id={book.id} name={book.name} img={book.img} 
                width={225} height={225} redirect={RedirectEnum.book} />
            })}
            </InfiniteScroll>
        // </div>
    }
        </>        
    )
}