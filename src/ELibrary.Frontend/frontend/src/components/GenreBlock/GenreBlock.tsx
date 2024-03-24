import { useNavigate } from 'react-router-dom'
import styles from './GenreBlock.module.css'
import { client } from '../../client/client'
import { ChangeEvent, useState } from 'react'
import { GenreDto } from '../../dto/Genre/GenreDto'

interface IGenreBlockProps {
    id: number,
    name: string
    isAdminHere: boolean
}

export function GenreBlock(props: IGenreBlockProps) {
    const [value, setValue] = useState<string>(props.name)
    const navigate = useNavigate()
    const [isEdit, setEdit] = useState<boolean>(false)
    const onDelete = async (id: number) => {
        // e.preventDefault()
        await client.delete('genre/delete-genre/' + id)
        .then(response => {
          console.log(response.data)
          window.location.reload()
        })
        .catch(error => {
          console.log(error.response.data)
        })
    }
    
    const onChange = (e: ChangeEvent<HTMLInputElement>) => {
        setValue(e.target.value)
    }

    const onSave = async () => {
        const dto: GenreDto = {
            id: props.id,
            name: value
        }
        await client.put('genre/change-genre', dto)
        .then(response => {
            setEdit(false)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const onCancel = (e) => {
        e.preventDefault()
        setEdit(false)
        setValue(props.name)
    }
    return (
        <div className={styles.genre_block}>
          {props.isAdminHere ? 
          <>
            {isEdit ?
            <>
                <input className={styles.input} onChange={onChange} value={value}/>
                <button className={styles.save_button} onClick={onSave}>Сохранить</button>
                <button className={styles.edit_button} onClick={onCancel}>Отменить</button>
            </> 
            :
            <>
                <p className={styles.genre_name} onClick={() => navigate('/book/?genres-ids=' + props.id)}>{value}</p>
                <button onClick={() => onDelete(props.id)} className={styles.delete_button}>Удалить жанр</button> 
                <button className={styles.edit_button} onClick={() => setEdit(true)}>Изменить</button>
            </>
            }          

          </>
          
          : <p className={styles.genre_name} onClick={() => navigate('/book/?genres-ids=' + props.id)}>{value}</p> }
        </div>
    )
}