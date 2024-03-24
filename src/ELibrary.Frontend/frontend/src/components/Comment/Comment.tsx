import { useNavigate } from "react-router-dom"
import { CommentDto, UpdateCommentDto } from "../../dto/Comment/Comment"
import { useAuth } from "../../hook/useAuth"
import styles from './Comment.module.css'
import { useEffect, useState } from "react"
import { client } from "../../client/client"
import { error } from "console"

interface ICommentProps {
    comment: CommentDto
}

export function Comment(props: ICommentProps) {
    const [comment, setCom] = useState<string>(props.comment.content)
    const [edit, setEdit] = useState<boolean>(false)
    const [isAdminHere, setIsAdmin] = useState<boolean>(false)
    const authStore = useAuth()
    const havigate = useNavigate()
    const edit_style = {
        height: 180 + 'px'
    }
    const non_edit_style = {
        height: 140 + 'px'
    }

    const onSave = async (e) => {
        e.stopPropagation()
        const dto: UpdateCommentDto = {
            commentId: props.comment.id,
            content: comment
        }
        await client.put('comment/update-comment', dto) 
        .then(response => {
            console.log(response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
        setEdit(false)
    }

    const onDelete = async (e) => {
        e.stopPropagation()
        await client.delete('comment/delete-comment/' + props.comment.id)
        .then(response => {
            console.log(response.data)
            window.location.reload()
        })
        .catch(error => {
            console.log(error.response.data)
        })

    }

    const onEdit = () => {
        if (authStore.isAuth && authStore.user.Id == props.comment.userId) {
            setEdit(true)
        } else if (authStore.isAuth && authStore.user.Role == 'admin') {
            setIsAdmin(true)
        }
    }

    const onCancel = (e) => {
        e.stopPropagation()
        setEdit(false)
        setIsAdmin(false)
        setCom(props.comment.content)
    }

    const changeComment = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setCom(e.target.value)
    }

    return(
        <div style={edit || isAdminHere ? edit_style : non_edit_style} key={props.comment.id} className={styles.comment} onClick={onEdit}>
            <img src={props.comment.userIcon.img} className={styles.user_icon} />
            <div className={styles.comment_content}>
                <p className={styles.name}>{props.comment.userIcon.name}</p>
                {edit ? 
                <>
                    <textarea defaultValue={comment} onChange={changeComment} className={styles.content} maxLength={250}></textarea>
                    <div className={styles.edit_panel}>
                        <button className={styles.edit_button} onClick={onSave}>Сохранить</button>
                        <button className={styles.edit_button} onClick={onCancel}>Отменить</button>
                        <button className={styles.delete_button} onClick={onDelete}>Удалить</button>
                    </div>
                </>
                : isAdminHere ?
                    <>
                        <textarea value={comment} className={styles.content} readOnly></textarea>                    
                        <button className={styles.edit_button} onClick={onCancel}>Отменить</button>
                        <button className={styles.delete_button} onClick={onDelete}>Удалить</button>
                    </>                    
                : <textarea value={comment} className={styles.content} readOnly></textarea>
                }
                {/* <textarea defaultValue={props.comment.content} className={styles.content} readOnly></textarea>
                <div className={styles.edit_panel}>
                    <button className={styles.edit_button} onClick={onSave}>Сохранить</button>
                    <button className={styles.edit_button} onClick={onCancel}>Отменить</button>
                    <button className={styles.delete_button} onClick={onDelete}>Удалить</button>
                </div> */}
            </div>
        </div>
    )
}