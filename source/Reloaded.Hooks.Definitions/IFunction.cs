﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.Helpers;

namespace Reloaded.Hooks.Definitions
{
    /// <summary>
    /// An interface for performing operations on native functions in memory.
    /// </summary>
    public interface IFunction<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(Trimming.ReloadedAttributeTypes)]
#endif
    TFunction>
    {
        /// <summary>
        /// Address of the function in memory.
        /// </summary>
        long Address { get; }

        /// <summary>
        /// Provides an interface to the hooks library.
        /// </summary>
        IReloadedHooks Hooks { get; }

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        IHook<TFunction> Hook(TFunction function, int minHookLength);

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        IHook<TFunction> Hook(TFunction function);

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        unsafe IHook<TFunction> Hook(void* function, int minHookLength);

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        unsafe IHook<TFunction> Hook(void* function);

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// Use only in .NET 5 and above with methods declared [UnmanagedCallersOnly].
        /// </summary>
        /// <param name="type">The type containing the method. Use "typeof()"</param>
        /// <param name="methodName">The name of the method. Use nameof()</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        unsafe IHook<TFunction> Hook(
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.Methods)]
#endif
            Type type, string methodName, int minHookLength);

        /// <summary>
        /// Creates a hook for a function at a given address.
        /// Use only in .NET 5 and above with methods declared [UnmanagedCallersOnly].
        /// </summary>
        /// <param name="type">The type containing the method. Use "typeof()"</param>
        /// <param name="methodName">The name of the method. Use nameof()</param>
        unsafe IHook<TFunction> Hook(
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.Methods)]
#endif
            Type type, string methodName);

        /// <summary>
        /// Creates a hook for this function using an alternative delegate/function pointer specified by <typeparamref name="TFunctionType"/>.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        unsafe IHook<TFunctionType> HookAs<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.ReloadedAttributeTypes)]
#endif
        TFunctionType>(void* function, int minHookLength);

        /// <summary>
        /// Creates a hook for this function using an alternative delegate/function pointer specified by <typeparamref name="TFunctionType"/>.
        /// </summary>
        /// <param name="function">The function to detour the original function to.</param>
        unsafe IHook<TFunctionType> HookAs<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.ReloadedAttributeTypes)]
#endif
        TFunctionType>(void* function);

        /// <summary>
        /// Creates a hook for this function using an alternative delegate/function pointer specified by <typeparamref name="TFunctionType"/>.
        /// Use only in .NET 5 and above with methods declared [UnmanagedCallersOnly].
        /// </summary>
        /// <param name="type">The type containing the method. Use "typeof()"</param>
        /// <param name="methodName">The name of the method. Use nameof()</param>
        /// <param name="minHookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        unsafe IHook<TFunctionType> HookAs<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.ReloadedAttributeTypes)]
#endif
        TFunctionType>(
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.Methods)]
#endif
            Type type, string methodName, int minHookLength);

        /// <summary>
        /// Creates a hook for this function using an alternative delegate/function pointer specified by <typeparamref name="TFunctionType"/>.
        /// Use only in .NET 5 and above with methods declared [UnmanagedCallersOnly].
        /// </summary>
        /// <param name="type">The type containing the method. Use "typeof()"</param>
        /// <param name="methodName">The name of the method. Use nameof()</param>
        unsafe IHook<TFunctionType> HookAs<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.ReloadedAttributeTypes)]
#endif
        TFunctionType>(
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(Trimming.Methods)]
#endif
            Type type, string methodName);

        /// <summary>
        /// Creates a wrapper function which allows you to call a function with a custom calling
        /// convention using the convention of <typeparamref name="TFunction"/>.
        /// </summary>
        /// <param name="wrapperAddress">Native address of the wrapper used to call the original function.</param>
        /// <remarks>The return value of this function is cached. Multiple calls will return same value.</remarks>
        /// <returns>A delegate for the function, ready to be called.</returns>
        TFunction GetWrapper(out IntPtr wrapperAddress);

        /// <summary>
        /// Creates a wrapper function which allows you to call a function with a custom calling
        /// convention using the convention of <typeparamref name="TFunction"/>.
        /// </summary>
        /// <remarks>The return value of this function is cached. Multiple calls will return same value.</remarks>
        /// <returns>A delegate for this function, ready to be called.</returns>
        TFunction GetWrapper();

        /// <summary>
        /// Creates a cheat engine style hook, replacing instruction(s) with a JMP to a user provided set of ASM instructions (and optionally the original ones).
        /// </summary>
        /// <param name="asmCode">
        ///     The assembly code to execute, in FASM syntax.
        ///     (Should start with use32/use64)
        /// </param>
        /// <param name="behaviour">Defines what should be done with the original code that was replaced with the JMP instruction.</param>
        /// <param name="hookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>

        IAsmHook MakeAsmHook(string[] asmCode, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1);

        /// <summary>
        /// Creates a cheat engine style hook, replacing instruction(s) with a JMP to a user provided set of ASM instructions (and optionally the original ones).
        /// </summary>
        /// <param name="asmCode">The assembly code to execute, precompiled.</param>
        /// <param name="behaviour">Defines what should be done with the original code that was replaced with the JMP instruction.</param>
        /// <param name="hookLength">Optional explicit length of hook. Use only in rare cases where auto-length check overflows a jmp/call opcode.</param>
        IAsmHook MakeAsmHook(byte[] asmCode, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1);
    }
}
