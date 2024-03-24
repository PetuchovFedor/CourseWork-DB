import { useEffect, useState } from "react"
import { CommentDto } from "../../dto/Comment/Comment"
import { useAuth } from "../../hook/useAuth"
import { Comment } from "../Comment/Comment"
import styles from './CommentsPanel.module.css'
import { client } from "../../client/client"
import InfiniteScroll from "react-infinite-scroll-component"

interface ICommentsPanelProps {
    bookId: number
    comments: CommentDto[]
}

export function CommentsPanel(props: ICommentsPanelProps) {
    const [countComs, setCountComs] = useState<number>(0)
    const [value, setValue] = useState<string>('')
    const [hasMore, setHasMore] = useState<boolean>(true)
    const [scipped, setScipped] = useState<number>(0)
    const [comments, setComments] = useState<CommentDto[]>(props.comments)
    // const commentsList = props.comments.map(comment => {
    //     return <Comment comment={comment}/>
    // })

    const authStore = useAuth()

    useEffect(() => {
        const getCount = async () => {
            console.log(props)
            await client.get<number>('comment/get-count-by-book-id/' + props.bookId)
            .then(response => {
                console.log(response.data)
                if (response.data === 0) {
                    setHasMore(false)
                }
                setCountComs(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
        getCount()
    }, [])
    const onChangeHandler = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setValue(e.target.value)
    }

    const createComment = async () => {
        const dto = {userId: authStore.user.Id, bookId: props.bookId, content: value}
        await client.post('comment/create-comment', dto)
        .then(response => {
            console.log(response.data)
            window.location.reload()
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const loadMore = async () => {
        setScipped(scipped + 10)
        if (scipped > countComs) {
            setHasMore(false)
            return
        }
        await client.get<CommentDto[] | string>('comment/get-part-comments/' + props.bookId + 
            '?scipped=' + scipped)
        .then(response => {
            response.data == 'comments over' ? setHasMore(false) : setComments(comments.concat(response.data))            
            console.log(response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    return (
        <div className={styles.panel}>
            <h2>Комментарии</h2>
            <div className={styles.add_comment_block}>
                <p className={styles.label}>Прокомментировать</p>
                {authStore.isAuth ? 
                <>
                <textarea  maxLength={250} className={styles.textarea} defaultValue={value} 
                onChange={onChangeHandler}></textarea>
                <button onClick={createComment} className={styles.button}>Отправить</button>
                </>
                :
                <p>Чтобы оставлять комментари нужно авторизоваться</p>}
            </div>
            <InfiniteScroll
                dataLength={comments.length}
                next={loadMore}
                hasMore={hasMore}
                loader={null}>

                {comments.map(comment => {
                    return <Comment comment={comment}/>
                })}
            </InfiniteScroll>
        </div>
    )
}