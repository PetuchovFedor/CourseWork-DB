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
import { GenreRouting } from './routing/GenreRouting';
import { UserRouting } from './routing/UserRouting';
import { BookRouting } from './routing/BookRouting';
import { useAuth } from './hook/useAuth';
import { SearchPage } from './pages/SearchPage/SearchPage';

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
