import * as nprogress from './js/nprogress';
import './scss/styles.scss';

(function (){
    nprogress.configure({showSpinner: false, parent: '#nprogress-container'});
    window.NProgress = nprogress;
})()