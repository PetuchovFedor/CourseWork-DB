import React from 'react';
import logo from './logo.svg';
import './App.css';
import {Main} from './pages/Main/Main'
import { observer } from 'mobx-react-lite'
import { LKS} from './pages/LKS/LKS';
import { GenresPage } from './pages/GenresPage/GenresPage';
import { BookPage } from './pages/BookPage/BookPage';
import { AuthorPage } from './pages/AuthorPage/AuthorPage';
import { AuthPage } from './pages/AuthPage/AuthPage';
import { RegistrationPage } from './pages/RegistrationPage/RegistrationPage';
import { AddBookPage } from './pages/AddBookPage/AddBookPage';
import * as pages from './pages/GenrePage/GenrePage';
import avatar from './images/avatar.png'
import cover from './images/cover1.jpg'
import {
  BrowserRouter,
  Routes,
  Route,
} from 'react-router-dom';
import { Header } from './components/Header/Header';
import { GenrePage } from './pages/GenrePage/GenrePage';
import { GenreRouting } from './routing/GenreRouting';
import { UserRouting } from './routing/UserRouting';
import { BookRouting } from './routing/BookRouting';
import { useAuth } from './hook/useAuth';
import { SearchPage } from './pages/SearchPage/SearchPage';

const ann:string = `В typescript мы будем определять многострочные строки с помощью шаблонных литералов. 
Шаблонный литерал - это не что иное, как строка, которая определяется обратными метками (“) вместо двойных и одинарных кавычек.
Здесь вы можете увидеть пример определения многострочной строки с использованием литералов шаблонов. 
Напишите приведенный ниже код в файле MultiLineString.ts.`

function App() {
  return (
    <BrowserRouter>
      <Header />
      <Routes>
          <Route path="/" element={<Main/> } />
          <Route path='auth' element={<AuthPage />} />
          <Route path='user/*' element={<UserRouting />} />
          <Route path='genre/*' element={<GenreRouting />} />
          <Route path='registration' element={<RegistrationPage />} />          
          <Route path='lks' element={<LKS />} />
          <Route path='add_book' element={<AddBookPage arr={[]}/>} />
          <Route path='book/*' element={<BookRouting />}/>
          <Route path='search' element={<SearchPage />} />
      </Routes>
    </BrowserRouter>
  );

}

export default observer(App);
