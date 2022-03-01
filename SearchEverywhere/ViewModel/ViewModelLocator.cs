﻿using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace SearchEverywhere.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel mainViewModel => Ioc.Default.GetService<MainViewModel>();
    }
}