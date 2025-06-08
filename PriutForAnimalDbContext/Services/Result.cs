using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Services
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Успешный результат не может содержать ошибку");
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Неуспешный результат должен содержать ошибку");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            return new Result<T>(default!, false, message); // Используем null-forgiving operator
        }

        public static Result Ok() => new Result(true, string.Empty);

        public static Result<T> Ok<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return new Result<T>(value, true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Нельзя получить Value из неуспешного результата");
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            if (isSuccess && value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }
    }
}
