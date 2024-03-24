import { redirect, useNavigate, useParams } from "react-router-dom";
import { Header } from "../../components/Header/Header";
import styles from './BookPage.module.css'
import { ChangeEvent, useEffect, useRef, useState } from "react";
import { useAuth } from "../../hook/useAuth";
import { client } from "../../client/client";
import { BookDto, ChangeFileDto, EditBookDto } from "../../dto/Book/BookDto";
import { CommentsPanel } from "../../components/CommentsPanel/CommentsPanel";
import { RatingDto } from "../../dto/RatingDto/RatingDto";
import { NotFound } from "../../components/NotFound/NotFound";
import { GenresMenu } from "../../components/GenresMenu/GenresMenu";
import { error } from "console";

export function BookPage() {
    const [isFound, setFound] = useState<boolean>(true)
    const [data, setData] = useState<BookDto>()
    const [genres, setGenres] = useState<number[]>()
    const [isEditGenres, setEditGenres] = useState<boolean>(false)
    const [name, setName] = useState<string>()
    const [annotation, setAnnotation] = useState<string>()
    const [isEditable, setIsEditable] = useState<boolean>(false)
    const [ratingValue, setRatingValue] = useState<number | undefined>(undefined)
    const [avgRating, setAvgRating] = useState<number>(0)
    const [prevRatingValue, setPrevRatingValue] = useState<number | undefined>(undefined)
    const [isEditRating, setIsEditRating] = useState<boolean>(false)
    const [isAdminHere, setIsAdmin] = useState<boolean>(false)
    const [isEdit, setEdit] = useState<boolean>(false)
    const [isInFavor, setInFavor] = useState<boolean>(false)
    const {id} = useParams()
    const navigate = useNavigate()
    const authStore = useAuth()
    const imgInputRef = useRef<HTMLInputElement>(null)
    const bookInputRef = useRef<HTMLInputElement>(null)    
    useEffect(() => {
        const getData = async () => {
            await client.get<BookDto>('book/' + id)
            .then(response => {
                console.log(response.data)
                setData(response.data)
                setGenres(response.data.genres.map(genre=>{return genre.id}))
                setName(response.data.name)
                setAvgRating(response.data.rating)
                setAnnotation(response.data.annotation)
                if (authStore.isAuth) {
                    // console.log(data?.authors.some((user) => {return user.id == authStore.user.Id}))
                    // console.log(authStore.user.Role == 'admin')
                    response.data?.authors.some((user) => {return user.id == authStore.user.Id}) ?
                     setIsEditable(true)
                    : setIsEditable(false)
                    authStore.user.Role == 'admin' ? setIsAdmin(true) : setIsAdmin(false)
                }
            })
            .catch(error => {
                if (error.response!.status === 404) {
                    setFound(false)
                }
                console.log(error.response.data)
            })
        }          
        getData()        // if (authStore.isAuth) {
        //     data?.authors.some((user) => {return user.id == authStore.user.Id}) && authStore.user.Role == 'admin' ? setIsEditable(true)
        //     : setIsEditable(false)
        // }
        // console.log(isEditable)
        
    }, [])

    useEffect(() => {
        const checkFavor = async () => {
            await client.get('user/check-read-book/?userId=' + authStore.user.Id + '&bookId=' + data.id)
            .then(response => {
                response.data == true ? setInFavor(true) : setInFavor(false)
                console.log(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }      
        if (authStore.isAuth) {
            checkFavor()
        }
        
    }, [data])
    //const printAuthors = () => {
    const authorsStr = <p className={styles.author_string}>{data?.authors.map((author, i: number) => {
        return <span>{i == 0 ? null : <span>, </span>}
        <span onClick={() => {navigate('/user/' + author.id)}}>{author.name}</span></span>
    })}</p>    

    const genresStr = <p className={styles.genre_string}>{data?.genres.map((genre, i: number) => {
        return <span>{i == 0 ? null : <span>, </span>}
        <span onClick={() => navigate('/book/?genres-ids=' + genre.id)}>{genre.name}</span></span>
    })}</p>
    //}

    const download = async () => {
        await client.get('book/get-book-file/' + data?.id, {responseType: 'blob'})
        .then(response => {
            // console.log(response.data)
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', data?.name + '.fb2');
            document.body.appendChild(link);
            link.click();
            //link.parentNode.removeChild(link);
    // clean up "a" element & remove ObjectURL
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        })
        .catch(error => {
            console.log(error.response.data)
        })
    } 

    const changeName = (e: React.ChangeEvent<HTMLInputElement>) => {
        setName(e.target.value)
    }

    const changeAnnotation = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setAnnotation(e.target.value)
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

    const onChangeCover = async () => {
        if (imgInputRef.current && imgInputRef.current.files) {
            const img = imgInputRef.current.files[0]
            const dto: ChangeFileDto = {id: data?.id, file: img};
            await client.put('book/change-cover', dto, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then(response => {
                console.log(response.data)
                window.location.reload()
            })
            .catch(error => {
                console.log(error.response.data)
            })
            //setBook(book)
        } 
    }

    const onChangeBook = async () => {
        if (bookInputRef.current && bookInputRef.current.files) {
            const book = bookInputRef.current.files[0]
            const dto: ChangeFileDto = {id: data?.id, file: book};
            await client.put('book/change-file', dto, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
            .then(response => {
                console.log(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
            //setBook(book)
        } 
    }

    const onSaveEdit = async (e) => {
        e.preventDefault()        
        const dto: EditBookDto = {id: data?.id, name: name, annotation: annotation}
        await client.put('book/edit-book', dto)
        .then(response => {
            console.log(response.data)
            setData({...data, name: name, annotation: annotation})
            // window.location.reload()
            setEdit(false)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }
    
    const clickOnRating = async () => {
        if (authStore.isAuth) {
            setIsEditRating(!isEditRating)
        }
        if (!isEditRating) {
            await client.get<RatingDto | number>('rating/get-rating/?userId=' + authStore.user.Id +
            '&bookId=' + data?.id)
            .then(response => {
                console.log(response.data)
                if (response.data === -1) {
                    setRatingValue(undefined) 
                    setPrevRatingValue(undefined)
                    prevRatingValue = undefined
                } else {
                    setRatingValue(response.data.mark)
                    setPrevRatingValue(ratingValue)
                } 
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
    }

    const changeRating = async (e: ChangeEvent<HTMLInputElement>) => {
        // console.log(e.target.value)
        // console.log(e.currentTarget.value)
        setRatingValue(e.target.value)
        // console.log(ratingValue)
        const dto: RatingDto = {
            userId: authStore.user.Id,
            bookId: data?.id,
            mark: e.target.value
        }
        // console.log(dto)
        if (prevRatingValue === undefined) {
            await client.post('rating/add-rating', dto)
            .then(response => {
                console.log(response.data)
                setPrevRatingValue(e.target.value)
                setAvgRating(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        } else {
            await client.put('rating/update-rating', dto)
            .then(response => {
                console.log(response.data)
                setAvgRating(response.data)
            })
            .catch(error => {
                console.log(error.response.data)
            })
        }
    }

    const deleteRating = async (e) => {
        e.preventDefault()        
        await client.delete('rating/delete-rating/?userId=' + authStore.user.Id +
        '&bookId=' + data?.id)
        .then(response => {
            setAvgRating(response.data)
            setRatingValue(undefined)
            setIsEditRating(false)
            setPrevRatingValue(undefined)            
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const onDelete = async (e) => {
        e.preventDefault()        
        await client.delete('book/delete/' + data.id)
        .then(response => {
            console.log(response.data)
            navigate(-1)
        })
        .catch(error => {           
            console.log(error.response.data)
        })
    }

    const onAdd = async (e) => {
        e.preventDefault()
        if (!authStore.isAuth) {
            navigate('/auth')
        }
        const dto = {
            userId: authStore.user.Id,
            bookId: data?.id
        }        
        await client.post('user/read-book', dto)
        .then(response => {
            console.log(response.data)
            setInFavor(true)
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const deleteFromFavor = async (e) => {
        e.preventDefault()
        await client.delete('user/delete-read-book/?userId=' + authStore.user.Id + '&bookId=' + data.id) 
        .then(response => {
            setInFavor(false)
            
        })
        .catch(error => {
            console.log(error.response.data)
        })
    }

    const onSaveEditGenres = async (e) => {
        e.preventDefault()
        const dto = {
            bookId: data?.id,
            genresId: genres
        }
        await client.put('book/change-genres', dto)
        .then(response => {
            setEditGenres(false)
            window.location.reload()
        })
        .catch(error => {
            console.log(error)
        })
    }
    return (        
        <>
        {
            isFound ?    
            <>     
            <input ref={imgInputRef} onChange={onChangeCover} type= 'file' accept=".jgp,.jpeg,.png" style={{'display': 'none'}} />
            <input ref={bookInputRef} onChange={onChangeBook} type ='file' accept=".fb2" style={{'display': 'none'}} required />
            {/* <p>{id}</p> */}
            {/* <Header /> */}
            <div className={styles.content}>
                <div className={styles.content_left}>
                    <img src={data?.cover} className={styles.cover} />
                    {/* <div className={styles.cover} style={{backgroundImage: 'url(' + data?.cover + ')'}}></div> */}
                    <div className={styles.left_buttons}>
                        <div className={styles.left_buttons_row}>
                            <button onClick={download} className={styles.download_button}>Скачать</button>
                            {isEditable ? <button className={styles.change_file_button} onClick={openSelectBookModal} >Изменить файл</button> 
                            : null}
                        </div>
                        {isInFavor ? <button onClick={deleteFromFavor} style={{background: '#ff0000'}} className={styles.delete_favor}>
                            Удалить из закладок</button> : 
                        <button onClick={onAdd} className={styles.add_button}>Добавить в закладки</button>}
                        {isEditable ? 
                        <>
                        <div className={styles.left_buttons_row}>
                            <button style={{background: '#ff0000'}}  onClick={onDelete} 
                            className={styles.delete_button}>Удалить</button>
                            <button onClick={openSelectImageModal} className={styles.change_cover_button}>Изменить обложку</button>
                        </div>
                        {isEdit ? 
                        <div className={styles.left_buttons_row}>
                            <button onClick={onSaveEdit} className={styles.save_button}>Сохранить</button>
                            <button className={styles.cancel_button} onClick={() => {setEdit(false)}}>Отмена</button>
                        </div>                        
                        : <button className={styles.edit_button} onClick={() => {setEdit(true)}}>Редактировать</button>}
                        {isEditGenres ?
                         <div className={styles.left_buttons_row}>
                            <button onClick={onSaveEditGenres} className={styles.save_button}>Сохранить</button>
                            <button className={styles.cancel_button} onClick={() => {setEditGenres(false)}}>Отмена</button>
                        </div>
                        : 
                        <button className={styles.edit_genres_button} onClick={()=> setEditGenres(true)}>Изменить жанры</button>}
                        </>                        
                        :  isAdminHere ? 
                        <div className={styles.left_buttons_row}>
                            <button style={{background: '#ff0000'}} onClick={onDelete} className={styles.delete_button}>Удалить</button>                        
                        </div>
                        : null}
                    </div>
                </div>
                <div className={styles.content_right}>
                    {isEdit ? 
                    <input type="text" className={styles.book_name_edit}
                    onChange={changeName} defaultValue={name} />
                    :
                    <h2 className={styles.book_name}>{data?.name}</h2>
                    }
                    {authorsStr}
                    {isEditGenres ? <GenresMenu id={5} selectedGenres={genres} setGenres={setGenres}/> : genresStr}
                    <p className={styles.rating}>Рейтинг: {isEditRating 
                    ? 
                      <>
                        <input type='number' min={0} max={10} className={styles.rating_input} value={ratingValue} onChange={changeRating}/>
                        <span onClick={clickOnRating}>/{Math.round(avgRating * 100) / 100 }</span>
                        <button onClick={deleteRating} className={styles.delete_rating_button}>Удалить оценку</button>
                      </>                    
                    : <span onClick={clickOnRating}>{Math.round(avgRating * 100) / 100 }</span>
                    }</p>
                    <p className={styles.publication_date}>Дата публикации: {data?.publicationDate === undefined ? '' : data?.publicationDate.substring(0, 10)}</p>
                    {isEdit ? 
                    <textarea className={styles.annotation_edit} value={annotation}
                    onChange={changeAnnotation} maxLength={500}></textarea>
                    :   
                    <textarea name="annotation" readOnly className={styles.annotation}
                    defaultValue={data?.annotation}></textarea>
                    }                   
                </div>
            </div>
            <CommentsPanel comments={data?.comments === undefined ? [] : data.comments} bookId={id} /> </>
            :
            <NotFound /> }
        </>
    )
}