﻿using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using MediatR;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Response<ObjectId>>
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public List<Interests> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<ObjectId>>
        {
            private readonly IUserRepositoryAsync _userRepository;
            public UpdateUserCommandHandler(IUserRepositoryAsync userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<Response<ObjectId>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(command.Id);

                if (user == null) throw new EntityNotFoundException("user", command.Id);

                var isUniqueEmail = await _userRepository.IsUniqueEmailAsync(command.Email);

                if (!isUniqueEmail) throw new EmailIsNotUniqueException(command.Email);

                /*
                 public string Name { get; set; }
public string Location { get; set; }
public List<string> Interests { get; set; }
public string PreferredLanguage { get; set; }
public string Email { get; set; }
public string Phone { get; set; }*/
                user.Name = command.Name;
                user.Email = command.Email;
                user.Phone = command.Phone;
                user.Place = command.Place;
                user.Interests = command.Interests;
                user.PreferredLanguage = command.PreferredLanguage;

                await _userRepository.UpdateAsync(user);
                return new Response<ObjectId>(user.Id);
            }
        }
    }
}
