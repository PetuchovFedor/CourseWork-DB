import { useEffect, useRef, useState } from "react";
import { Header } from "../../components/Header/Header";
import styles from './AddBookPage.module.css'
import { GenreDto } from "../../dto/Genre/GenreDto";
import { client } from "../../client/client";
import { useAuth } from "../../hook/useAuth";
import { useNavigate } from "react-router-dom";
import {GenresMenu} from "../../components/GenresMenu/GenresMenu";
import { CreateBookDto } from "../../dto/Book/BookDto";
import { FilterAndSortingPanel } from "../../components/FilterAndSortingPanel/FilterAndSortingPanel";
import { SortState } from "../../components/FilterAndSortingPanel/SortState";

// interface AddBook {
//     name: string;
//     annotation: string;
//     authors: string[]
//     coverPath: string;
//     bookPath: string;
// }

interface props {
    arr: number[]
}
export function AddBookPage(props: props) {
    const [genresId, setIds] = useState<Array<number>>([])
    const [name, setName] = useState<string>('')
    const [authors, setAuthors] = useState<string>('')
    const [ann, setAnn] = useState<string>('')
    const [cover, setCover] = useState<File>()
    const [book, setBook] = useState<File>()
    const authStore = useAuth()
    const navigate = useNavigate()
    const imgInputRef = useRef<HTMLInputElement>(null)
    const bookInputRef = useRef<HTMLInputElement>(null)

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) =>{
        console.log(genresId)
        e.preventDefault()
        if (cover === undefined || book === undefined) {
            alert('Выберите файлы')
            return
        }
        if (genresId.length == 0) {
            alert('Выберите жанры')
            return
        }
        let temp: string[] = []
        authors == '' ? temp.push(authStore.user.Name) : temp = authors.split(',')
        // console.log(authStore.user)
        const dto: CreateBookDto = {
            name: name,
            authors: temp,
            annotation: ann,
            genres: genresId,
            cover: cover,
            bookFile: book
        }
        console.log(dto)
        await client.post('book/create', dto, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        
        .then(response => {
            console.log(response.data)
            navigate('/book/' + response.data)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const onChangeName = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault()
        setName(e.target.value)
    }

    const onChangeAuthors = (e: React.ChangeEvent<HTMLInputElement>) => {
        setAuthors(e.target.value)
    }

    const onChangeAnnotation = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setAnn(e.target.value)
    }

    const openSelectImageModal = () => {
        if (imgInputRef.current) {
            imgInputRef.current.click()
        }
    }

    const openSelectBookModal = () => {
        if (bookInputRef.current) {
            bookInputRef.current.click()
        }
    }

    const onChangeCover = () => {
        if (imgInputRef.current && imgInputRef.current.files) {
            const img = imgInputRef.current.files[0]
            setCover(img)
        } 
    }

    const onChangeBook = () => {
        if (bookInputRef.current && bookInputRef.current.files) {
            const book = bookInputRef.current.files[0]
            setBook(book)
        } 
    }

    return(
        <>
        <form className={styles.form} onSubmit={handleSubmit}>            
            <h2>Добавление книги</h2>
            <input name={'name'} className={styles.form_input} placeholder="Введите название книги" onChange={onChangeName} required />                
            <label className={styles.form_elem}>Введите авторов, через запятую если их несколько: </label>
            <input name = {'authors'} className={styles.form_input} onChange={onChangeAuthors}
             placeholder="Поле необязательно если автор один"/>            
            <label>Напишите аннотацию</label>
            <textarea className={styles.textarea} name="ann" onChange={onChangeAnnotation} required maxLength={500}></textarea>
            {/* <FilterAndSortingPanel sortState={SortState.None} genres={[]} /> */}
            <div>
            <GenresMenu id={1} setGenres={setIds} selectedGenres={props.arr} />
            </div>
            <div className={styles.form_elem}>
                <button className={styles.button} onClick={openSelectImageModal}>Выбрать обложку</button>
                <button className={styles.button} onClick={openSelectBookModal}>Выбрать книгу</button>
            </div>
            <div className={styles.form_elem}>
                <button className={styles.button} type="submit">Добавить</button>
                <button className={styles.button} onClick={() => navigate('/')}>Отмена</button>
            </div>
            <input ref={imgInputRef} onChange={onChangeCover} type= 'file' accept=".jgp,.jpeg,.png" style={{'display': 'none'}} />
            <input ref={bookInputRef} onChange={onChangeBook} type ='file' accept=".fb2" style={{'display': 'none'}} required />
        </form>
        </>
    )
}